//  
//  File Name   :   Game.cs
//


// Namespaces
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Game : Singleton2<Game>
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


    GameMenuBehaviour GameMenuBehaviour
    {
        get { return (m_gameMenu.GetComponent<GameMenuBehaviour>()); }
    }


// Member Fields


    const float k_startDelay        = 6.0f;

    public GameObject m_gameMenu    = null;

    EState m_currentState           = EState.INVALID;
    EState m_pausedOnState          = EState.INVALID;

    float m_startTimer              = k_startDelay;


// Member Methods


    public void Pause()
    {
        if (m_currentState != EState.StartCountDown &&
            m_currentState != EState.Playing)
            return;

        GameMenuBehaviour.SetPanel(GameMenuBehaviour.EPanel.Pause);

        m_pausedOnState = m_currentState;
        m_currentState = EState.Paused;

        // Notify observers
        if (EventPause != null)
            EventPause(this);
    }


    public void Resume()
    {
        if (m_currentState != EState.Paused)
            return;

        GameMenuBehaviour.SetPanel(GameMenuBehaviour.EPanel.InGame);

        m_currentState = m_pausedOnState;

        // Notify observers
        if (EventResume != null)
            EventResume(this);
    }


    public void GameOver()
    {
        if (m_currentState == EState.GameOver)
            return;

        GameMenuBehaviour.SetPanel(GameMenuBehaviour.EPanel.GameOver);

        m_currentState = EState.GameOver;

        // Notify observers
        if (EventGameOver != null)
            EventGameOver(this);
    }


    public void ResetAndPlay()
    {
        // Notify observers
        if (EventRestart != null)
            EventRestart(this);

        m_startTimer = k_startDelay;

        GameMenuBehaviour.SetPanel(GameMenuBehaviour.EPanel.InGame);

        GameMenuBehaviour.CountdownLabelVisible = true;

        m_currentState = EState.StartCountDown;
    }


    public void QuitToMainMenu()
    {
        Application.LoadLevel("MainMenu");
    }


    void Start()
    {
        InitGameMenu();

        // Start game
        ResetAndPlay();
    }


    void OnDestroy()
    {
        // Empty
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


    void InitGameMenu()
    {
        GameMenuBehaviour gameMenuBehaviour = GameMenuBehaviour;

        gameMenuBehaviour.EventPauseButtonPress     += OnEventPauseButtonPress;
        gameMenuBehaviour.EventResumeButtonPress    += OnEventResumeButtonPress;
        gameMenuBehaviour.EventRestartButtonPress   += OnEventRestartButtonPress;
        gameMenuBehaviour.EventQuitButtonPress      += OnEventQuitButtonPress;
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
            GameMenuBehaviour.CountdownLabelVisible = false;

            m_currentState = EState.Playing;

            // Notify observers
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


};
