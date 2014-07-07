//  
//  File Name   :   EnemyGruntBehaviour.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyGruntBehaviour : EnemyBehaviour
{

// Member Types


// Member Delegates & Events


// Member Properties


    public override float MovementSpeed { get; set; }


// Member Fields
	

// Member Methods


    void Awake()
    {
        base.Awake();

        MovementSpeed = 4;
    }


	void Start()
	{
        base.Start();
	}


	void OnDestroy()
	{
        base.OnDestroy();
	}


	void Update()
	{
        base.Update();
	}


};
