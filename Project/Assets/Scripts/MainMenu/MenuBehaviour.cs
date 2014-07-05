//  
//  File Name   :   MenuBehaviour.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuBehaviour : MonoBehaviour
{

// Member Types


    public enum EPanel
    {
        INVALID,
        Main,
        Settings,
        Controls,
        Credits
    }


// Member Delegates & Events


    public delegate void OnPanelChangeHandler(MenuBehaviour _sender, EPanel _ePanel);
    public event OnPanelChangeHandler EventPanelChanged;


// Member Properties


// Member Fields


    public UIPanel m_panelMain          = null;
    public UIPanel m_panelSettings      = null;
    public UIPanel m_panelControls      = null;
    public UIPanel m_panelCredits       = null;


    EPanel m_activePanel                = EPanel.INVALID;


// Member Methods


    public void SetActivePanel(EPanel _ePanel)
    {
        switch (_ePanel)
        {
            case EPanel.Main:
                m_panelMain.gameObject.SetActive(true);
                m_panelSettings.gameObject.SetActive(false);
                m_panelControls.gameObject.SetActive(false);
                m_panelCredits.gameObject.SetActive(false);
                break;

            case EPanel.Settings:
                m_panelMain.gameObject.SetActive(false);
                m_panelSettings.gameObject.SetActive(true);
                m_panelControls.gameObject.SetActive(false);
                m_panelCredits.gameObject.SetActive(false);
                break;

            case EPanel.Controls:
                m_panelMain.gameObject.SetActive(false);
                m_panelSettings.gameObject.SetActive(true);
                m_panelControls.gameObject.SetActive(true);
                m_panelCredits.gameObject.SetActive(false);
                break;

            case EPanel.Credits:
                m_panelMain.gameObject.SetActive(false);
                m_panelSettings.gameObject.SetActive(true);
                m_panelControls.gameObject.SetActive(false);
                m_panelCredits.gameObject.SetActive(true);
                break;

            default:
                Debug.LogError("Unknown panel: " + _ePanel);
                break;
        }

        //Debug.Log(string.Format("Change menu panel from ({0}) to ({1})", m_activePanel, _ePanel));

        m_activePanel = _ePanel;

        // Notify observers
        if (EventPanelChanged != null)
            EventPanelChanged(this, m_activePanel);
    }


    public void OnPlayButtonPress()
    {
        Application.LoadLevel("Game");
    }


    public void OnSettingsButtonPress()
    {
        SetActivePanel(EPanel.Settings);
    }


    public void OnCloseSettingsButtonPress()
    {
        SetActivePanel(EPanel.Main);
    }


    public void OnSoundButtonPress()
    {
    }


    public void OnCreditsButtonPress()
    {
    }


    void Start()
    {
        // Set default menu
        SetActivePanel(EPanel.Main);
    }


    void OnDestroy()
    {
        // Empty
    }


    void Update()
    {
        // Empty
    }


};
