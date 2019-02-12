using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelegator
{
    // Delagate
    public delegate void MovementChangeHandler(string name);
    public event MovementChangeHandler MovementChangeEvent;
    public delegate void ActionChangeHandler(string name);
    public event ActionChangeHandler ActionChangeEvent;
    public delegate void SpeedValueChangeHandler(float speed);
    public event SpeedValueChangeHandler SpeedValueChangeEvent;
    public delegate void PlayIconHandler(bool isPlaying);
    public event PlayIconHandler PlayIconEvent;


    public AnimationDelegator()
    {
    }

    public void InvokeMovementChange(string name)
    {
        MovementChangeEvent.Invoke(name);
    }

    public void InvokeActionChange(string name)
    {
        ActionChangeEvent.Invoke(name);
    }

    public void InvokeSpeedValueChange(float lastSpeed)
    {
        if (SpeedValueChangeEvent != null)
            SpeedValueChangeEvent.Invoke(lastSpeed);
    }

    public void InvokePlayIcon(bool isPlaying)
    {
        if (PlayIconEvent != null)
            PlayIconEvent.Invoke(isPlaying);
    }
}
