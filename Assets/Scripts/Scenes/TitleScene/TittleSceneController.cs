using System.Collections;
using System.Collections.Generic;
using ActionSample.Components.Ui;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TittleSceneController : MonoBehaviour
{
    enum STATES
    {
        INITIAL,
        MAIN,
        LEAVE
    }

    private STATES state;

    private SceneTransition sceneTransitionHandler;

    void Start()
    {
        state = STATES.INITIAL;

    }

    void FixedUpdate()
    {
        switch (state)
        {
            case STATES.INITIAL:
                var startButton = transform.GetComponentInChildren<StartButton>();
                startButton.Subscribe(this.HandleStartButtonClick);

                sceneTransitionHandler = transform.GetComponentInChildren<SceneTransition>();
                sceneTransitionHandler.SubscribeFadeOut(HandleFadeOutEnd);
                sceneTransitionHandler.Begin(SceneTransition.MODE.FADEIN, 2.0f);
                state = STATES.MAIN;
                break;

            case STATES.LEAVE:
                SceneManager.LoadScene("SampleScene");
                break;
        }
    }


    void HandleStartButtonClick()
    {
        sceneTransitionHandler.Begin(SceneTransition.MODE.FADEOUT);
    }


    void HandleFadeOutEnd()
    {
        state = STATES.LEAVE;
    }
}
