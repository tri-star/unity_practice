using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    private Animator _animator;

    public void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnMouseDown()
    {
        Debug.Log("Pressed");
        _animator.SetBool("isPressed", true);
    }

    public void OnMouseUp()
    {
        _animator.SetBool("isPressed", false);
    }

}
