using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class StartButton : MonoBehaviour, IPointerDownHandler
{
    private UnityEvent ev;

    public void Start()
    {
        ev = new UnityEvent();
        GetComponent<Animator>().SetBool("isPressed", false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Animator>().SetBool("isPressed", true);
        ev.Invoke();
    }

    public void Subscribe(UnityAction action)
    {
        ev.AddListener(action);
    }
}
