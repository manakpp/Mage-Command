using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class ObjectPool : MonoBehaviour
{
	static ObjectPool _instance;

	Dictionary<Component, List<Component>> objectLookup = new Dictionary<Component, List<Component>>();
	Dictionary<Component, Component> prefabLookup = new Dictionary<Component, Component>();
	List<Component> toBeRemoved = new List<Component>();

	GameObject m_activeGameobjectsParent;
	
	public static void Clear()
	{
		Instance.objectLookup.Clear();
		Instance.prefabLookup.Clear();
	}

	public static void Restart()
	{
		foreach (var keyPair in Instance.prefabLookup)
		{
			var obj = keyPair.Key;
			obj.gameObject.SetActive(false);
			obj.transform.parent = _instance.transform;

			// Return to pool
			Instance.objectLookup[Instance.prefabLookup[keyPair.Key]].Add(obj);

			Instance.toBeRemoved.Add(obj);
		}

		foreach (var obj in Instance.toBeRemoved)
		{
			Instance.prefabLookup.Remove(obj);	
		}

		Instance.toBeRemoved.Clear();
	}

	public static void CreatePool<T>(T _prefab, int _startBuffer = 1) where T : Component
	{
		if (!Instance.objectLookup.ContainsKey(_prefab))
		{
			var objectBuffer = new List<Component>();

			if (!Instance.objectLookup.ContainsKey(_prefab))
			{
				Instance.objectLookup[_prefab] = objectBuffer;

				for (int i = 0; i < _startBuffer; ++i)
				{
					var obj = Instantiate(_prefab, Vector3.one * 1000.0f, Quaternion.identity) as T;
					objectBuffer.Add(obj);

					obj.transform.parent = Instance.transform;
				}
			}
		}
	}
	
	public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, bool _mustBeFromPool = true) where T : Component
	{
		if (Instance.objectLookup.ContainsKey(prefab))
		{
			T obj = null;

			var list = Instance.objectLookup[prefab];
			if (list.Count > 0)
			{
				while (obj == null && list.Count > 0)
				{
					obj = list[0] as T;
					list.RemoveAt(0);
				}

				if (obj != null)
				{
					obj.transform.parent = Instance.m_activeGameobjectsParent.transform;
					obj.transform.localPosition = position;
					obj.transform.localRotation = rotation;
					obj.gameObject.SetActive(true);
					Instance.prefabLookup.Add(obj, prefab);

					return (T)obj;
				}
			}

			if (_mustBeFromPool)
				return null;

			obj = (T)Object.Instantiate(prefab, position, rotation);
			Instance.prefabLookup.Add(obj, prefab);

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
		if (Instance.prefabLookup.ContainsKey(obj))
		{
			Instance.objectLookup[Instance.prefabLookup[obj]].Add(obj);
			Instance.prefabLookup.Remove(obj);
			obj.transform.parent = Instance.transform;
			obj.gameObject.SetActive(false);
		}
		else
		{
			Debug.Log(typeof(T));
			Object.Destroy(obj.gameObject);
		}
	}

	public static void EnterActiveGroup(Component _obj)
	{
		_obj.transform.parent = Instance.m_activeGameobjectsParent.transform;
	}

	public static int Count<T>(T prefab) where T : Component
	{
		if (Instance.objectLookup.ContainsKey(prefab))
			return Instance.objectLookup[prefab].Count;
		else
			return 0;
	}

	public static ObjectPool Instance
	{
		get
		{
			if (_instance != null)
				return _instance;

			var obj = new GameObject("_ObjectPool");
			obj.transform.localPosition = Vector3.zero;
			_instance = obj.AddComponent<ObjectPool>();

			_instance.m_activeGameobjectsParent = new GameObject("_ActivePooledObjects");
			_instance.m_activeGameobjectsParent.transform.localPosition = Vector3.zero;

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


	public static void EnterActiveGroup<T>(this T obj) where T : Component
	{
		ObjectPool.EnterActiveGroup(obj);
	}


	public static int Count<T>(T prefab) where T : Component
	{
		return ObjectPool.Count(prefab);
	}
}
