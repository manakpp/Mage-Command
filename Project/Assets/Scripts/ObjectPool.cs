//  
//  File Name   :   ObjectPool.cs
//  Adapted from : 
// http://forum.unity3d.com/threads/simple-reusable-object-pool-help-limit-your-instantiations.76851/


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : Singleton<ObjectPool>
{

// Member Types


	[System.Serializable]
	public struct TPooledObject
	{
		public EObjectPool type;
		public GameObject prefab;
		public int amountToBuffer;
	}


	public enum EObjectPool
	{
		Projectile,
	}


// Member Properties

	public Dictionary<EObjectPool, List<GameObject>> PooledObjects
	{
		get { return m_pooledObjects; }
	}


// Member Fields

	/// <summary>
	/// The object prefabs which the pool can handle.
	/// </summary>
	public TPooledObject[] m_objectsToBePooled;

	public int m_defaultBufferAmount = 3;

	/// <summary>
	/// The container object that we will keep unused pooled objects so we dont clog up the editor with objects.
	/// </summary>
	protected GameObject m_containerObject;

	/// <summary>
	/// The pooled objects currently available.
	/// </summary>
	private Dictionary<EObjectPool, List<GameObject>> m_pooledObjects;



// Member Methods


	void Start()
	{
		m_containerObject = new GameObject("ObjectPool");

		//Loop through the object prefabs and make a new list for each one.
		//We do this because the pool can only support prefabs set to it in the editor,
		//so we can assume the lists of pooled objects are in the same order as object prefabs in the array
		m_pooledObjects = new Dictionary<EObjectPool, List<GameObject>>(m_objectsToBePooled.Length);

		foreach (TPooledObject pooledObject in m_objectsToBePooled)
		{
			// Figure out amount to buffer
			int bufferAmount = pooledObject.amountToBuffer;
			if (pooledObject.amountToBuffer <= 0)
			{
				bufferAmount = m_defaultBufferAmount;
			}

			// Create new list if required
			if (!m_pooledObjects.ContainsKey(pooledObject.type))
				m_pooledObjects[pooledObject.type] = new List<GameObject>();
			
			// Create buffered amount of objects
			for (int n = 0; n < bufferAmount; n++)
			{
				GameObject newObj = Instantiate(pooledObject.prefab) as GameObject;
				newObj.name = pooledObject.prefab.name;
				PoolObject(pooledObject.type, newObj);
			}
		}
	}

	/// <summary>
	/// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
	/// then null will be returned.
	/// </summary>
	/// <returns>
	/// The object for type.
	/// </returns>
	/// <param name='objectType'>
	/// Object type.
	/// </param>
	/// <param name='_onlyPooled'>
	/// If true, it will only return an object if there is one currently pooled.
	/// </param>
	public GameObject GetObjectOfType(EObjectPool _pool, bool _onlyPooled = true)
	{
		if(!m_pooledObjects.ContainsKey(_pool))
			return null;

		var pool = m_pooledObjects[_pool];

		if (pool.Count > 0)
		{
			GameObject pooledObject = pool[0];
			pool.RemoveAt(0);
			pooledObject.transform.parent = null;

			return pooledObject;
		}
		else if (!_onlyPooled)
		{
			GameObject toInstantiate = null;

			foreach (TPooledObject pooledObject in m_objectsToBePooled)
			{
				if (pooledObject.type == _pool)
				{
					toInstantiate = pooledObject.prefab;
					break;
				}
			}

			if (toInstantiate == null)
				return null;

			GameObject newObj = Instantiate(toInstantiate) as GameObject;
			newObj.name = toInstantiate.name;

			return newObj;
		}

		//If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
		return null;
	}


	/// <summary>
	/// Pools the object specified.  Will not be pooled if there is no prefab of that type.
	/// </summary>
	/// <param name='obj'>
	/// Object to be pooled.
	/// </param>
	public void PoolObject(EObjectPool _pool, GameObject _obj)
	{
		for (int i = 0; i < m_objectsToBePooled.Length; i++)
		{
			// Make sure this object is something that is pooled
			if (m_objectsToBePooled[i].prefab.name == _obj.name)
			{
				_obj.transform.parent = m_containerObject.transform;
				m_pooledObjects[_pool].Add(_obj);
				return;
			}
		}
	}
}