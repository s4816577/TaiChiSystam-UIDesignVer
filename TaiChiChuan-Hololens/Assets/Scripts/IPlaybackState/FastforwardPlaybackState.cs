using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastforwardPlayback : IPlaybackState
{
    private Director director;
    private AnimationManager animationManager;

    private const float SKIPPED_SPEED = 1.0f;
    private float targetTime;

    public FastforwardPlayback(Director director, AnimationManager animationManager, float targetTime)
    {
        this.director = director;
        this.animationManager = animationManager;
        this.targetTime = targetTime;

        animationManager.SetSpeed(/*SKIPPED_SPEED*/animationManager.LastSpeed);
    }

    public void Update()
    {
        // Check ending condition.
        if (animationManager.CurrentTime >= targetTime)
        {
            animationManager.SetAnimationTime(targetTime);
            animationManager.CurrentTime = targetTime;
            director.SetPlaybackState(new NormalPlaybackState(director, animationManager));
        }
    }

    public void Play()
    {
        // Do nothing.
    }

    public void Pause()
    {
        // Do nothing.
    }

    public void Restart()
    {
        // Do nothing.
    }

    public void SpeedUp()
    {
        // Do nothing.
    }

    public void SpeedDown()
    {
        // Do nothing.
    }

    public void Next()
    {

    }

    public void Last()
    {

    }

    public void NextMovement()
    {
        // Do nothing.
    }

    public void LastMovement()
    {
        // Do nothing.
    }

    public void NextAction()
    {
        // Do nothing.
    }

    public void LastAction()
    {
        // Do nothing.
    }

    public bool CanPlayActionAudio()
    {
        return false;
    }

	public void SetRestartInd(int Ind)
	{
		//Do nothing
	}
}

