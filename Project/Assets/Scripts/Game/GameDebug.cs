//  
//  File Name   :   CLASSNAME.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameDebug : MonoBehaviour
{

// Member Types


// Member Delegates & Events


// Member Properties


// Member Fields


// Member Methods


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
        if (Input.GetKey(KeyCode.RightAlt))
        {
            Game.Instance.GameOver();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Game.Instance.ResetAndPlay();
        }
    }


};
