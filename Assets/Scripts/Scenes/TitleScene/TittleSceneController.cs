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

    private SceneTransitionHandler sceneTransitionHandler;

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

                sceneTransitionHandler = transform.GetComponentInChildren<SceneTransitionHandler>();
                sceneTransitionHandler.SubscribeFadeOut(HandleFadeOutEnd);
                sceneTransitionHandler.Begin(SceneTransitionHandler.MODE.FADEIN, 2.0f);
                state = STATES.MAIN;
                break;

            case STATES.LEAVE:
                SceneManager.LoadScene("SampleScene");
                break;
        }
    }


    void HandleStartButtonClick()
    {
        sceneTransitionHandler.Begin(SceneTransitionHandler.MODE.FADEOUT);
    }


    void HandleFadeOutEnd()
    {
        state = STATES.LEAVE;
    }
}
