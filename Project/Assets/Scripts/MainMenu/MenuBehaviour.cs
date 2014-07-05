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


    public struct TButtonColours
    {
        public Color normal;
        public Color hover;
        public Color press;
    }


// Member Delegates & Events


    public delegate void OnPanelChangeHandler(MenuBehaviour _sender, EPanel _ePanel);
    public event OnPanelChangeHandler EventPanelChanged;


// Member Properties


// Member Fields


    public UIPanel m_panelMain              = null;
    public UIPanel m_panelSettings          = null;
    public UIPanel m_panelSettingsOptions   = null;
    public UIPanel m_panelControls          = null;
    public UIPanel m_panelCredits           = null;

    public UIButton m_buttonSound = null;

    EPanel m_activePanel                    = EPanel.INVALID;

    TButtonColours m_soundBtnDefaultColours;


// Member Methods


    public void SetActivePanel(EPanel _ePanel)
    {
        switch (_ePanel)
        {
            case EPanel.Main:
                m_panelMain.gameObject.SetActive(true);
                m_panelSettings.gameObject.SetActive(false);
                break;

            case EPanel.Settings:
                m_panelMain.gameObject.SetActive(false);
                m_panelSettings.gameObject.SetActive(true);
                m_panelSettingsOptions.gameObject.SetActive(true);
                m_panelControls.gameObject.SetActive(false);
                m_panelCredits.gameObject.SetActive(false);
                break;

            case EPanel.Controls:
                m_panelMain.gameObject.SetActive(false);
                m_panelSettings.gameObject.SetActive(true);
                m_panelSettingsOptions.gameObject.SetActive(false);
                m_panelControls.gameObject.SetActive(true);
                m_panelCredits.gameObject.SetActive(false);
                break;

            case EPanel.Credits:
                m_panelMain.gameObject.SetActive(false);
                m_panelSettings.gameObject.SetActive(true);
                m_panelSettingsOptions.gameObject.SetActive(false);
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
        if (Settings.SoundEnabled)
        {
            Settings.SoundEnabled = false;
        }
        else
        {
            Settings.SoundEnabled = true;
        }

        RefreshSoundButtonState();
    }


    public void OnControlsButtonPress()
    {
        SetActivePanel(EPanel.Controls);
    }


    public void OnCreditsButtonPress()
    {
        SetActivePanel(EPanel.Credits);
    }


    public void OnBackButtonPress()
    {
        SetActivePanel(EPanel.Settings);
    }


    void Start()
    {
        // Set default menu
        SetActivePanel(EPanel.Main);

        // Save sound button default colours
        m_soundBtnDefaultColours.normal = m_buttonSound.defaultColor;
        m_soundBtnDefaultColours.hover = m_buttonSound.hover;
        m_soundBtnDefaultColours.press = m_buttonSound.pressed;

        RefreshSoundButtonState();
    }


    void OnDestroy()
    {
        // Empty
    }


    void Update()
    {
        // Empty
    }


    void RefreshSoundButtonState()
    {
        // Restore default colours
        if (Settings.SoundEnabled)
        {
            m_buttonSound.defaultColor  = m_soundBtnDefaultColours.normal;
            m_buttonSound.hover         = m_soundBtnDefaultColours.hover;
            m_buttonSound.pressed       = m_soundBtnDefaultColours.press; 
        }

        // Set disabled colours
        else
        {
            m_buttonSound.defaultColor  = Color.red;
            m_buttonSound.hover         = Color.red;
            m_buttonSound.pressed       = Color.red;
        }

        m_buttonSound.SetState(UIButtonColor.State.Normal, false);
    }


};
