//  
//  File Name   :   Game.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Game : MonoBehaviour
{

// Member Types


    public enum EState
    {
        INVALID,
        StartCountDown,
        Playing,
        Paused,
        GameOver
    }


// Member Delegates & Events


    public delegate void OnStartHandler(Game _sender);
    public event OnStartHandler EventStart;


    public delegate void OnRestartHandler(Game _sender);
    public event OnRestartHandler EventRestart;


    public delegate void OnGameOverHandler(Game _sender);
    public event OnGameOverHandler EventGameOver;


    public delegate void OnPauseHandler(Game _sender);
    public event OnPauseHandler EventPause;


    public delegate void OnResumeHandler(Game _sender);
    public event OnResumeHandler EventResume;


// Member Properties

	public Mage Mage
	{
		get { return m_mage; }
	}


	public Grid Grid
	{
		get { return m_groundGrid; }
	}


	public GameController Controller
	{
		get;
		set;
	}


	public static Game Instance
	{
		get { return s_instance; }
	}


	GameMenuBehaviour GameMenuBehaviour
	{
		get { return (m_gameMenu.GetComponent<GameMenuBehaviour>()); }
	}


    float StartTimer
    {
        get { return (m_startTimer); }
    }


// Member Fields


    const float k_startDelay        = 6.0f;

    public GameObject m_gameMenu    = null;
	
	Grid m_groundGrid				= null;
	Mage m_mage						= null;

    EState m_currentState           = EState.INVALID;
    EState m_pausedOnState          = EState.INVALID;

    float m_startTimer              = k_startDelay;

	static Game s_instance          = null;


// Member Methods


    public void Pause()
    {
        if (m_currentState != EState.StartCountDown &&
            m_currentState != EState.Playing)
            return;

        GameMenuBehaviour.SetPanel(GameMenuBehaviour.EPanel.Pause);

        m_pausedOnState = m_currentState;
        m_currentState = EState.Paused;

        if (EventPause != null)
            EventPause(this);
    }


    public void Resume()
    {
        if (m_currentState != EState.Paused)
            return;

        GameMenuBehaviour.SetPanel(GameMenuBehaviour.EPanel.InGame);

        m_currentState = m_pausedOnState;

        if (EventResume != null)
            EventResume(this);
    }


    public void GameOver()
    {
        if (m_currentState == EState.GameOver)
            return;

        GameMenuBehaviour.SetPanel(GameMenuBehaviour.EPanel.GameOver);

        m_currentState = EState.GameOver;

        if (EventGameOver != null)
            EventGameOver(this);
    }


    public void ResetAndPlay()
    {
        if (EventRestart != null)
            EventRestart(this);

        // Reset start timer
        m_startTimer = k_startDelay;

        // Set menu settings
        GameMenuBehaviour.SetPanel(GameMenuBehaviour.EPanel.InGame);
        GameMenuBehaviour.CountdownLabelVisible = true;

        m_currentState = EState.StartCountDown;

        // Reset object pool
		ObjectPool.Restart();
    }


    public void QuitToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }


	void Awake()
	{
		s_instance = this;

		Controller = GetComponent<GameController>();
	}


    void Start()
    {
        InitialiseGameMenu();

        // Start game
        ResetAndPlay();

		m_groundGrid = GameObject.FindGameObjectWithTag("GroundGrid").GetComponent<Grid>();
		// Sign up to stats change
        m_mage                     = GameObject.FindGameObjectWithTag("Mage").GetComponent<Mage>();
		m_mage.EventManaChanged   += OnEventMageManaChanged;
        m_mage.EventHealthChanged += OnEventMageHealthChanged;
    }


	void OnDestroy()
	{
		m_mage.EventManaChanged   -= OnEventMageManaChanged;
        m_mage.EventHealthChanged -= OnEventMageHealthChanged;
	}


    void Update()
    {
        switch (m_currentState)
        {
            case EState.StartCountDown:
                UpdateStateStartCountDown();
                break;

            case EState.Playing:
                UpdateStatePlaying();
                break;

            case EState.Paused:
                UpdateStatePaused();
                break;

            case EState.GameOver:
                UpdateStateGameOver();
                break;

            default:
                Debug.LogError("Unknown state: " + m_currentState);
                break;
        }
    }


    void InitialiseGameMenu()
    {
        GameMenuBehaviour.EventPauseButtonPress   += OnEventPauseButtonPress;
        GameMenuBehaviour.EventResumeButtonPress  += OnEventResumeButtonPress;
        GameMenuBehaviour.EventRestartButtonPress += OnEventRestartButtonPress;
        GameMenuBehaviour.EventQuitButtonPress    += OnEventQuitButtonPress;
    }


    void UpdateStateStartCountDown()
    {
        float m_previousTime = m_startTimer;
        m_startTimer -= Time.deltaTime;

        if (m_startTimer > 1.0f)
        {
            GameMenuBehaviour.CountdownLabelText = Mathf.Floor(m_startTimer).ToString();
        }
        else if (m_startTimer > 0.0f)
        {
            GameMenuBehaviour.CountdownLabelText = "Start!";
        }
        else
        {
            // Start game
            GameMenuBehaviour.CountdownLabelVisible = false;

            m_currentState = EState.Playing;

            if (EventStart != null)
                EventStart(this);
        }
    }


    void UpdateStatePlaying()
    {
        // Empty
    }


    void UpdateStatePaused()
    {
        // Empty
    }


    void UpdateStateGameOver()
    {
        // Empty
    }


    void OnEventPauseButtonPress(GameMenuBehaviour _sender)
    {
        Pause();
    }


    void OnEventResumeButtonPress(GameMenuBehaviour _sender)
    {
        Resume(); 
    }


    void OnEventRestartButtonPress(GameMenuBehaviour _sender)
    {
        ResetAndPlay();
    }


    void OnEventQuitButtonPress(GameMenuBehaviour _sender)
    {
        QuitToMainMenu();
    }


	void OnEventMageManaChanged(Mage _sender, float _currentMana, float _maxMana, float _prevMana)
	{
		GameMenuBehaviour.SetManaRatio(_currentMana / _maxMana);
	}


	void OnEventMageHealthChanged(Mage _sender, int _currentHealth, int _maxHealth, int _prevHealth)
	{
		GameMenuBehaviour.SetHealthRatio((float)_currentHealth / (float)_maxHealth);
		
		if (_currentHealth == 0)
			GameOver();
	}
};
