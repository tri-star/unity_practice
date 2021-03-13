using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    private enum States
    {
        WAIT,
        INPROGRESS,
        DONE
    }

    private States state;

    [SerializeField]
    private float fade;

    private float fadeSpeed;

    private UnityEvent endEvent = new UnityEvent();

    void Start()
    {
        var canvas = GetComponentInParent<Canvas>();
        var canvasRect = canvas.GetComponent<RectTransform>();

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.position = new Vector3(0, 0, 999);
        rectTransform.sizeDelta = new Vector2(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y);

        state = States.WAIT;
        fadeSpeed = 1.0f;

    }

    void FixedUpdate()
    {
        switch (state)
        {
            case States.WAIT:
                GetComponent<RectTransform>().position = new Vector3(0, 0, 999);
                fade = 0;
                UpdateAlpha(0);
                break;
            case States.INPROGRESS:
                fade += (1.0f / (60.0f * fadeSpeed));
                if (fade > 1)
                {
                    state = States.DONE;
                    endEvent.Invoke();
                }
                UpdateAlpha(fade);
                break;
            case States.DONE:
                break;
        }
    }

    public void StartFade()
    {
        GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        state = States.INPROGRESS;
        fade = 0;
        UpdateAlpha(fade);
    }

    public void SubscribeFadeEnd(UnityAction action)
    {
        endEvent.AddListener(action);
    }

    private void UpdateAlpha(float progress)
    {
        gameObject.GetComponent<Image>().color = new Color(255, 255, 255, progress);
    }
}
