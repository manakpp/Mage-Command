﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class ObjectPool : MonoBehaviour
{
	static ObjectPool _instance;

	Dictionary<Component, List<Component>> objectLookup = new Dictionary<Component, List<Component>>();
	Dictionary<Component, Component> prefabLookup = new Dictionary<Component, Component>();
	
	public static void Clear()
	{
		instance.objectLookup.Clear();
		instance.prefabLookup.Clear();
	}

	public static void Restart()
	{
		//foreach (KeyValuePair<Component, List<Component>> keyPair in _instance.objectLookup)
		//{
		//    foreach (var obj in keyPair.Value)
		//    {
		//        obj.Recycle();
		//    }
		//}
	}

	public static void CreatePool<T>(T _prefab, int _startBuffer = 1) where T : Component
	{
		if (!instance.objectLookup.ContainsKey(_prefab))
		{
			var objectBuffer = new List<Component>();

			if (!instance.objectLookup.ContainsKey(_prefab))
			{
				instance.objectLookup[_prefab] = objectBuffer;

				for (int i = 0; i < _startBuffer; ++i)
				{
					var obj = Instantiate(_prefab, Vector3.one * 1000.0f, Quaternion.identity) as T;
					objectBuffer.Add(obj);

					obj.transform.parent = instance.transform;
				}
			}
		}
	}
	
	public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, bool _mustBeFromPool = true) where T : Component
	{
		if (instance.objectLookup.ContainsKey(prefab))
		{
			T obj = null;

			var list = instance.objectLookup[prefab];
			if (list.Count > 0)
			{
				while (obj == null && list.Count > 0)
				{
					obj = list[0] as T;
					list.RemoveAt(0);
				}

				if (obj != null)
				{
					obj.transform.parent = null;
					obj.transform.localPosition = position;
					obj.transform.localRotation = rotation;
					obj.gameObject.SetActive(true);
					instance.prefabLookup.Add(obj, prefab);
					return (T)obj;
				}
			}

			if (_mustBeFromPool)
				return null;

			obj = (T)Object.Instantiate(prefab, position, rotation);
			instance.prefabLookup.Add(obj, prefab);

			return (T)obj;
		}
		else if (_mustBeFromPool)
		{
			return null;
		}

		return (T)Object.Instantiate(prefab, position, rotation);
	}
	public static T Spawn<T>(T prefab, Vector3 position, bool _mustBeFromPool = true) where T : Component
	{
		return Spawn(prefab, position, Quaternion.identity, _mustBeFromPool);
	}
	public static T Spawn<T>(T prefab, bool _mustBeFromPool = true) where T : Component
	{
		return Spawn(prefab, Vector3.zero, Quaternion.identity, _mustBeFromPool);
	}

	public static void Recycle<T>(T obj) where T : Component
	{
		if (instance.prefabLookup.ContainsKey(obj))
		{
			instance.objectLookup[instance.prefabLookup[obj]].Add(obj);
			instance.prefabLookup.Remove(obj);
			obj.transform.parent = instance.transform;
			obj.gameObject.SetActive(false);

		}
		else
		{
			Object.Destroy(obj.gameObject);
		}
	}

	public static int Count<T>(T prefab) where T : Component
	{
		if (instance.objectLookup.ContainsKey(prefab))
			return instance.objectLookup[prefab].Count;
		else
			return 0;
	}

	public static ObjectPool instance
	{
		get
		{
			if (_instance != null)
				return _instance;
			var obj = new GameObject("_ObjectPool");
			obj.transform.localPosition = Vector3.zero;
			_instance = obj.AddComponent<ObjectPool>();
			return _instance;
		}
	}
}

public static class ObjectPoolExtensions
{
	public static void CreatePool<T>(this T prefab) where T : Component
	{
		ObjectPool.CreatePool(prefab);
	}


	public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation, bool _mustBeFromPool = true) where T : Component
	{
		return ObjectPool.Spawn(prefab, position, rotation, _mustBeFromPool);
	}


	public static T Spawn<T>(this T prefab, Vector3 position, bool _mustBeFromPool = true) where T : Component
	{
		return ObjectPool.Spawn(prefab, position, Quaternion.identity, _mustBeFromPool);
	}


	public static T Spawn<T>(this T prefab, bool _mustBeFromPool = true) where T : Component
	{
		return ObjectPool.Spawn(prefab, Vector3.zero, Quaternion.identity, _mustBeFromPool);
	}
	

	public static void Recycle<T>(this T obj) where T : Component
	{
		ObjectPool.Recycle(obj);
	}


	public static int Count<T>(T prefab) where T : Component
	{
		return ObjectPool.Count(prefab);
	}
}
