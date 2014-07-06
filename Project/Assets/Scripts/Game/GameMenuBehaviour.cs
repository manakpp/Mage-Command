//  
//  File Name   :   InGameMenuBehaviour.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameMenuBehaviour : MonoBehaviour
{

// Member Types


    public enum EPanel
    {
        INVALID,
        InGame,
        Pause,
        GameOver
    }


    public struct TButtonColours
    {
        public Color normal;
        public Color hover;
        public Color press;
    }


// Member Delegates & Events


    public delegate void OnPauseButtonPressHandler(GameMenuBehaviour _sender);
    public event OnPauseButtonPressHandler EventPauseButtonPress;


    public delegate void OnResumeButtonPressHandler(GameMenuBehaviour _sender);
    public event OnResumeButtonPressHandler EventResumeButtonPress;


    public delegate void OnRestartButtonPressHandler(GameMenuBehaviour _sender);
    public event OnRestartButtonPressHandler EventRestartButtonPress;


    public delegate void OnSoundToggleButtonPressHandler(GameMenuBehaviour _sender);
    public event OnSoundToggleButtonPressHandler EventSoundToggleButtonPress;


    public delegate void OnQuitButtonPressHandler(GameMenuBehaviour _sender);
    public event OnQuitButtonPressHandler EventQuitButtonPress;


// Member Properties


    public string CountdownLabelText
    {
        get { return (m_labelCountdown.text); }

        set { m_labelCountdown.text = value; }
    }


    public bool CountdownLabelVisible
    {
        get { return (m_labelCountdown.gameObject.activeInHierarchy); }

        set { m_labelCountdown.gameObject.SetActive(value); }
    }


// Member Fields


    public UIPanel m_panelGame              = null;
    public UIPanel m_panelPause             = null;
    public UIPanel m_panelGameOver          = null;

    public UIProgressBar m_progressbarMana  = null;

    public UIButton m_buttonSoundToggle     = null;

    public UILabel m_labelCountdown         = null;
    public UILabel m_labelWave              = null;


    TButtonColours m_soundBtnDefaultColours;
    
    EPanel m_activePanel                    = EPanel.INVALID;
	

// Member Methods


    public void SetPanel(EPanel _ePanel)
    {
        switch (_ePanel)
        {
            case EPanel.InGame:
                m_panelGame.gameObject.SetActive(true);
                m_panelPause.gameObject.SetActive(false);
                m_panelGameOver.gameObject.SetActive(false);
                break;

            case EPanel.Pause:
                m_panelGame.gameObject.SetActive(false);
                m_panelPause.gameObject.SetActive(true);
                m_panelGameOver.gameObject.SetActive(false);
                break;

            case EPanel.GameOver:
                m_panelGame.gameObject.SetActive(false);
                m_panelPause.gameObject.SetActive(false);
                m_panelGameOver.gameObject.SetActive(true);
                break;

            default:
                Debug.LogError("Unknown panel: " + _ePanel);
                break;
        }

        m_activePanel = _ePanel;
    }


    public void SetManaRatio(float _fRatio)
    {
        m_progressbarMana.value = _fRatio;
    }


    public void OnPauseButtonPress()
    {
        if (EventPauseButtonPress != null)
            EventPauseButtonPress(this);
    }


    public void OnResumeButtonPress()
    {
        if (EventResumeButtonPress != null)
            EventResumeButtonPress(this);
    }


    public void OnRestartButtonPress()
    {
        if (EventRestartButtonPress != null)
            EventRestartButtonPress(this);
    }


    public void OnSoundToggleButtonPress()
    {
        Settings.Instance.SoundEnabled = !Settings.Instance.SoundEnabled;

        if (EventSoundToggleButtonPress != null)
            EventSoundToggleButtonPress(this);
    }


    public void OnQuitButtonPress()
    {
        if (EventQuitButtonPress != null)
            EventQuitButtonPress(this);
    }


	void Start()
	{
        // Set default panel
        SetPanel(EPanel.InGame);

        // Sign up to sound change
        Settings.Instance.EventSoundChanged += OnEventSoundChanged;

        // Save sound button default colours
        m_soundBtnDefaultColours.normal = m_buttonSoundToggle.defaultColor;
        m_soundBtnDefaultColours.hover  = m_buttonSoundToggle.hover;
        m_soundBtnDefaultColours.press  = m_buttonSoundToggle.pressed;

        RefreshSoundToggleButton();
	}


	void OnDestroy()
	{
        // De-register from sound change
        Settings.Instance.EventSoundChanged -= OnEventSoundChanged;
	}


	void Update()
	{
		// Empty
	}


    void OnEventSoundChanged(Settings _sender, bool _bEnabled)
    {
        RefreshSoundToggleButton();
    }


    public void RefreshSoundToggleButton()
    {
        // Restore default colours
        if (Settings.Instance.SoundEnabled)
        {
            m_buttonSoundToggle.defaultColor    = m_soundBtnDefaultColours.normal;
            m_buttonSoundToggle.hover           = m_soundBtnDefaultColours.hover;
            m_buttonSoundToggle.pressed         = m_soundBtnDefaultColours.press;
        }

        // Set disabled colours
        else
        {
            m_buttonSoundToggle.defaultColor    = Color.red;
            m_buttonSoundToggle.hover           = Color.red;
            m_buttonSoundToggle.pressed         = Color.red;
        }

        m_buttonSoundToggle.SetState(UIButtonColor.State.Normal, false);
    }


};
