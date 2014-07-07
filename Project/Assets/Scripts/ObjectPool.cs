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

	public static void CreatePool<T>(T _component, int _startBuffer = 1) where T : Component
	{
		if (!Instance.objectLookup.ContainsKey(_component))
		{
			var objectBuffer = new List<Component>();

			if (!Instance.objectLookup.ContainsKey(_component))
			{
				Instance.objectLookup[_component] = objectBuffer;

				for (int i = 0; i < _startBuffer; ++i)
				{
					var obj = Instantiate(_component.gameObject, Vector3.one * 1000.0f, Quaternion.identity) as GameObject;

					var component = obj.GetComponent(typeof(T)) as T;

					objectBuffer.Add(component);

					obj.transform.parent = Instance.transform;
				}
			}
		}
	}
	
	public static T Spawn<T>(T _component, Vector3 position, Quaternion rotation, bool _mustBeFromPool = true) where T : Component
	{
		// Does this component exist in a pool?
		if (Instance.objectLookup.ContainsKey(_component))
		{
			T newComponent = null;

			// Get the pool
			var list = Instance.objectLookup[_component];
			if (list.Count > 0)
			{
				// Take one of the pooled components from the pool
				while (newComponent == null && list.Count > 0)
				{
					newComponent = list[0] as T;
					list.RemoveAt(0);
				}

				// Return the component
				if (newComponent != null)
				{
					newComponent.transform.parent = Instance.m_activeGameobjectsParent.transform;
					newComponent.transform.localPosition = position;
					newComponent.transform.localRotation = rotation;
					newComponent.gameObject.SetActive(true);
					Instance.prefabLookup.Add(newComponent, _component);

					return (T)newComponent;
				}
			}

			// There doesn't seem to be components left in the pool. If we must grab from pool then return now.
			if (_mustBeFromPool)
				return null;

			// Else add more to the pool and return it
			var obj = Instantiate(_component.gameObject, position, rotation) as GameObject;
			newComponent = obj.GetComponent(typeof(T)) as T;
			Instance.prefabLookup.Add(newComponent, _component);

			return (T)newComponent;
		}
		else if (_mustBeFromPool)
		{
			return null;
		}

		// This component does not belong to a prefab. Create it in a global space.
		var unpooledObject = Instantiate(_component.gameObject, position, rotation) as GameObject;
		var unpooledComponent = unpooledObject.GetComponent(typeof(T)) as T;

		return unpooledComponent;
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
