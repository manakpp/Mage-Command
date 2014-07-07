//  
//  File Name   :   EnemyArcherBehaviour.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyArcherBehaviour : EnemyBehaviour
{

// Member Types


// Member Delegates & Events


// Member Properties


    public override float MovementSpeed { get; set; }


// Member Fields
	

// Member Methods


    void Awake()
    {
        MovementSpeed = 4;
    }


	void Start()
	{
		// Empty
	}


	void OnDestroy()
	{
		// Empty
	}


	void Update()
	{
        base.Update();
	}


};
