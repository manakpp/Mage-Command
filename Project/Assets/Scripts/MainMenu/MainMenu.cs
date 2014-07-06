//  
//  File Name   :   MainMenu.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MainMenu : MonoBehaviour
{

// Member Types


// Member Delegates & Events


// Member Properties


    public static MainMenu Instance
    {
        get { return (m_instance); }
    }


    public static MenuBehaviour MenuBehaviour
    {
        get { return (Instance.m_menuBehaviour); }
    }


// Member Fields


    public MenuBehaviour m_menuBehaviour;

	
    static MainMenu m_instance;


// Member Methods


	void Awake()
	{
		m_instance = this;
	}

    void Start()
    {
        m_menuBehaviour.EventPlayButtonPress += OnEventPlayButtonPress;
    }


    void OnDestroy()
    {
        // Empty
    }


    void Update()
    {
        // Empty
    }


    void OnEventPlayButtonPress(MenuBehaviour _sender)
    {
        Application.LoadLevel("Game");
    }


};
