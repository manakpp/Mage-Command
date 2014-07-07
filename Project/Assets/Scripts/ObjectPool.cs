using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class ObjectPool : MonoBehaviour
{
	static ObjectPool s_instance;

	Dictionary<GameObject, List<GameObject>> m_objectLookup = new Dictionary<GameObject, List<GameObject>>();
	Dictionary<GameObject, GameObject> m_prefabLookup = new Dictionary<GameObject, GameObject>();
	List<GameObject> m_toBeRemoved = new List<GameObject>();

	GameObject m_activeGameobjectsParent;
	
	public static void Clear()
	{
		Instance.m_objectLookup.Clear();
		Instance.m_prefabLookup.Clear();
	}

	public static void Restart()
	{
		foreach (var keyPair in Instance.m_prefabLookup)
		{
			var obj = keyPair.Key;
			obj.gameObject.SetActive(false);
			obj.transform.parent = s_instance.transform;

			// Return to pool
			Instance.m_objectLookup[Instance.m_prefabLookup[keyPair.Key]].Add(obj);

			Instance.m_toBeRemoved.Add(obj);
		}

		foreach (var obj in Instance.m_toBeRemoved)
		{
			Instance.m_prefabLookup.Remove(obj);	
		}

		Instance.m_toBeRemoved.Clear();
	}

	public static void CreatePool(GameObject _prefab, int _startBuffer = 1)
	{
		// If it doesn't exist then create it
		if (!Instance.m_objectLookup.ContainsKey(_prefab))
		{
			var objectBuffer = new List<GameObject>();
			Instance.m_objectLookup[_prefab] = objectBuffer;

			// Populate with a buffer of objects
			for (int i = 0; i < _startBuffer; ++i)
			{
				var obj = Instantiate(_prefab, Vector3.one * 1000.0f, Quaternion.identity) as GameObject;
				obj.transform.parent = Instance.transform;

				objectBuffer.Add(obj);
			}
		}
	}

	public static GameObject Spawn(GameObject _prefab, Vector3 position, Quaternion rotation, bool _mustBeFromPool = true)
	{
		GameObject newGameObject = null;

		// Does this GameObject exist in a pool?
		if (Instance.m_objectLookup.ContainsKey(_prefab))
		{
			// Get the pool
			var list = Instance.m_objectLookup[_prefab];
			if (list.Count > 0)
			{
				// Take one of the pooled GameObjects from the pool
				while (newGameObject == null && list.Count > 0)
				{
					newGameObject = list[0];
					list.RemoveAt(0);
				}

				// Return the GameObject
				if (newGameObject != null)
				{
					newGameObject.transform.parent = Instance.m_activeGameobjectsParent.transform;
					newGameObject.transform.localPosition = position;
					newGameObject.transform.localRotation = rotation;
					newGameObject.gameObject.SetActive(true);
					Instance.m_prefabLookup.Add(newGameObject, _prefab);

					return newGameObject;
				}
			}

			// There doesn't seem to be GameObjects left in the pool. If we must grab from pool then return now.
			if (_mustBeFromPool)
				return null;

			// Else add more to the pool and return it
			newGameObject = Instantiate(_prefab, position, rotation) as GameObject;
			Instance.m_prefabLookup.Add(newGameObject, _prefab);

			return newGameObject;
		}
		
		// This GameObject does not belong to a prefab key. If it must be in a pool then return now
		if (_mustBeFromPool)
		{
			return null;
		}

		// Else create it in a global space.
		newGameObject = Instantiate(_prefab.gameObject, position, rotation) as GameObject;

		return newGameObject;
	}

	public static GameObject Spawn(GameObject _prefab, Vector3 _position, bool _mustBeFromPool = true)
	{
        return Spawn(_prefab, _position, _prefab.transform.rotation, _mustBeFromPool);
	}

	public static GameObject Spawn(GameObject _prefab, bool _mustBeFromPool = true)
	{
        return Spawn(_prefab, Vector3.zero, _prefab.transform.rotation, _mustBeFromPool);
	}

	public static void Recycle(GameObject _object)
	{
		// Find the prefab associated with this object
		if (Instance.m_prefabLookup.ContainsKey(_object))
		{
			// Add it back to the object pool
			Instance.m_objectLookup[Instance.m_prefabLookup[_object]].Add(_object);
			
			// Remove from active pool
			Instance.m_prefabLookup.Remove(_object);
			
			// Disable object
			_object.transform.parent = Instance.transform;
			_object.gameObject.SetActive(false);
		}
		else
		{
			// Does not exist in pool just remove it
			Object.Destroy(_object.gameObject);
		}
	}

	public static void EnterActiveGroup(GameObject _obj)
	{
		_obj.transform.parent = Instance.m_activeGameobjectsParent.transform;
	}

	public static int Count(GameObject _prefab)
	{
		if (Instance.m_objectLookup.ContainsKey(_prefab))
			return Instance.m_objectLookup[_prefab].Count;
		else
			return 0;
	}

	public static ObjectPool Instance
	{
		get
		{
			if (s_instance != null)
				return s_instance;

			var obj = new GameObject("_ObjectPool");
			obj.transform.localPosition = Vector3.zero;
			s_instance = obj.AddComponent<ObjectPool>();

			s_instance.m_activeGameobjectsParent = new GameObject("_ActivePooledObjects");
			s_instance.m_activeGameobjectsParent.transform.localPosition = Vector3.zero;

			return s_instance;
		}
	}
}