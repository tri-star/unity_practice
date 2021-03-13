using System.Collections;
using System.Collections.Generic;
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

    private FadeController fadeController;

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

                fadeController = transform.GetComponentInChildren<FadeController>();
                fadeController.SubscribeFadeEnd(HandleFadeEnd);
                state = STATES.MAIN;
                break;

            case STATES.LEAVE:
                SceneManager.LoadScene("SampleScene");
                break;
        }
    }


    void HandleStartButtonClick()
    {
        fadeController.StartFade();
    }


    void HandleFadeEnd()
    {
        state = STATES.LEAVE;
    }
}
