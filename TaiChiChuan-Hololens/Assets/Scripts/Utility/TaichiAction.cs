using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaichiAction
{
    // Index info.
    private int actionNumber;
    public int ActionNumber { get { return actionNumber; } }

    // Time info.
    private float normalizedBeginTime;
    public float NormalizedBeginTime { get { return normalizedBeginTime; } }
    private float normalizedEndTime;
    public float NormalizedEndTime { get { return normalizedEndTime; } set { normalizedEndTime = value; } }

    // Name and corresponding Sound.
    private string actionName;
    public string ActionName { get { return actionName; } }
    private AudioClip sound;
    public AudioClip Sound { get { return sound; } }

    public TaichiAction(int actionNumber, float normalizedBeginTime, string actionName, AudioClip sound)
    {
        this.actionNumber = actionNumber;
        this.normalizedBeginTime = normalizedBeginTime;
        this.actionName = actionName;
        this.sound = sound;
    }
}
