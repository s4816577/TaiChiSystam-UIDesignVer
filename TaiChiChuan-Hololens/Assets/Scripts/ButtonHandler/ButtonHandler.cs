using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

abstract public class ButtonHandler : MonoBehaviour, HoloToolkit.Unity.InputModule.IInputClickHandler
{
    protected Director director;

    // Use this for initialization
    protected virtual void Start()
    {
        director = UnityEngine.Object.FindObjectOfType<Director>();
    }

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        ProcessInputClicked(eventData);

        director.StartCapturingGlobalClicked();
        director.DestroyControlPanel();
    }

    protected abstract void ProcessInputClicked(InputClickedEventData eventData);
}
