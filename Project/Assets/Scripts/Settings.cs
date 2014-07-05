//  
//  File Name   :   Settings.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Settings
{

// Member Types


// Member Delegates & Events


// Member Properties


    public static bool SoundEnabled
    {
        get { return (PlayerPrefs.GetInt(k_soundEnabledKey, 1) == 1); }

        set { PlayerPrefs.SetInt(k_soundEnabledKey, (value) ? 1 : 0); }
    }


// Member Fields


    const string k_soundEnabledKey = "SoundEnabled";


// Member Methods


};
