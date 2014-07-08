//  
//  File Name   :   EnemyBehaviour.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class EnemyBehaviour : MonoBehaviour
{

// Member Types


    public enum EState
    {
        INVALID,
        MovingTowardsBarricade,
        AttackingBarricade,
    }


// Member Delegates & Events


// Member Properties


    public abstract float MovementSpeed { get; set; }


// Member Fields


    EState m_currentState = EState.INVALID;

	bool m_isDead;


// Member Methods


	public void Spawn()
	{
		m_isDead = false;
	}


    public void Kill()
    {
        m_isDead = true;
    }


    protected void Awake()
    {
        // Empty
    }


    protected void Start()
    {
        gameObject.SetActive(false);
    }


    protected void Update()
    {
        switch (m_currentState)
        {
            case EState.MovingTowardsBarricade:
                UpdateStateMovingTowardsTarget();
                break;

            case EState.AttackingBarricade:
                UpdateStateAttackingBarricade();
                break;

            default:
                Debug.LogError("Unknown state: " + m_currentState);
                break;

        }
    }


    protected void OnDestroy()
	{
		// Empty
	}


    void UpdateStateMovingTowardsTarget()
    {
        // Update movement
        transform.position += Vector3.left * MovementSpeed * Time.deltaTime;
    }


    void UpdateStateAttackingBarricade()
    {

    }


	void OnCollisionEnter(Collision _collision)
	{
		if (m_isDead)
			return;

		if(_collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
		{
			rigidbody.AddForce((transform.position - _collision.transform.position).normalized * 100.0f);
		}
	}


	void OnTriggerEnter(Collider _otherCollider)
	{

		// Game.Instance.Mage.Health -= 1;
	}


};
