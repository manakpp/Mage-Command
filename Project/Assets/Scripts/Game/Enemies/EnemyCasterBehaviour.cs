//  
//  File Name   :   EnemyCasterBehaviour.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyCasterBehaviour : EnemyBehaviour
{

// Member Types


// Member Delegates & Events


// Member Properties


    public override float MovementSpeed { get; set; }


// Member Fields
	

// Member Methods


	protected override void Awake()
    {
        base.Awake();

        MovementSpeed = 4;
    }


	protected override void Start()
	{
        base.Start();
	}


	protected override void OnDestroy()
	{
        base.OnDestroy();
	}


	protected override void Update()
	{
        base.Update();
	}


};
