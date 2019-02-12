using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaichiMovement
{
    // Index info.
    private int movementNumber;
    public int MovementNumber { get { return movementNumber; } }

    // Time info.
    private float normalizedBeginTime;
    public float NormalizedBeginTime { get { return normalizedBeginTime; } }
    private float normalizedEndTime;
    public float NormalizedEndTime { get { return normalizedEndTime; } set { normalizedEndTime = value; } }

    // Name and corresponding Sound.
    private string movementName;
    public string MovementName { get { return movementName; } }
    private AudioClip sound;
    public AudioClip Sound { get { return sound; } }

    // Taichi Acntion list.
    private List<TaichiAction> taichiActionArray;
    public List<TaichiAction> TaichiActionArray { get { return taichiActionArray; } }

    public TaichiMovement(int movementNumber, float normalizedBeginTime, string movementName, AudioClip sound)
    {
        this.movementNumber = movementNumber;
        this.normalizedBeginTime = normalizedBeginTime;
        this.movementName = movementName;
        this.sound = sound;
    }

    public void SetTaichiActionArray(List<TaichiAction> taichiActionArray)
    {
        this.taichiActionArray = taichiActionArray;
    }
}
