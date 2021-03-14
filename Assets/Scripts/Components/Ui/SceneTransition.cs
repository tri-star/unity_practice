using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ActionSample.Components.Ui
{
    public class SceneTransition : MonoBehaviour
    {
        public enum MODE
        {
            FADEIN,
            FADEOUT
        }

        private enum STATES
        {
            WAIT,
            INPROGRESS,
            DONE
        }

        private MODE mode;

        private STATES state;

        private float fade;

        [SerializeField]
        private float defaultFadeSpeed = 1.0f;

        private float fadeSpeed;

        private UnityEvent fadeInEvent = new UnityEvent();

        private UnityEvent fadeOutEvent = new UnityEvent();

        void Awake()
        {
            state = STATES.WAIT;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(640, 360);
        }

        void Update()
        {
            if (state != STATES.DONE)
            {
                gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            }
        }

        void FixedUpdate()
        {
            switch (state)
            {
                case STATES.WAIT:
                    mode = MODE.FADEOUT;
                    fadeSpeed = defaultFadeSpeed;
                    fade = 0;
                    UpdateAlpha(0);
                    break;
                case STATES.INPROGRESS:
                    fade += (1.0f / (60.0f * fadeSpeed));
                    if (fade > 1)
                    {
                        Complete();
                    }
                    UpdateAlpha(fade);
                    break;
                case STATES.DONE:
                    break;
            }
        }

        public void Begin(MODE mode, float speed = 0)
        {
            if (speed == 0)
            {
                speed = defaultFadeSpeed;
            }
            var rectTransform = GetComponent<RectTransform>();
            rectTransform.position = new Vector2(0, 0);

            this.mode = mode;
            state = STATES.INPROGRESS;
            fade = 0;
            UpdateAlpha(fade);
        }

        public void SubscribeFadeIn(UnityAction action)
        {
            fadeInEvent.AddListener(action);
        }

        public void SubscribeFadeOut(UnityAction action)
        {
            fadeOutEvent.AddListener(action);
        }


        private void UpdateAlpha(float progress)
        {
            if (mode == MODE.FADEIN)
            {
                progress = 1.0f - progress;
            }
            gameObject.GetComponent<Image>().color = new Color(255, 255, 255, progress);
        }

        private void Complete()
        {
            state = STATES.DONE;
            if (mode == MODE.FADEIN)
            {
                gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 999);
                fadeInEvent.Invoke();
            }
            else
            {
                fadeOutEvent.Invoke();
            }
        }
    }

}
