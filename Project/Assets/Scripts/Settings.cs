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


    public delegate void OnSoundChangeHandler(Settings _sender, bool _bEnabled);
    public event OnSoundChangeHandler EventSoundChanged;


// Member Properties


    public static Settings Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = new Settings();
            }

            return (s_instance);
        }
    }


    public bool SoundEnabled
    {
        get { return (PlayerPrefs.GetInt(k_soundEnabledKey, 1) == 1); }

        set 
        {
            if (SoundEnabled == value)
                return;

            PlayerPrefs.SetInt(k_soundEnabledKey, (value) ? 1 : 0); 

            // Notify observers
            if (EventSoundChanged != null)
                EventSoundChanged(this, SoundEnabled);
        }
    }


// Member Fields


    const string k_soundEnabledKey = "SoundEnabled";


    static Settings s_instance;


// Member Methods


    Settings()
    {
        // Empty
    }


};
