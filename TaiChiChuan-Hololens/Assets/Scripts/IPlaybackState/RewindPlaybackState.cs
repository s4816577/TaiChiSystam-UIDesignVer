using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindPlaybackState : IPlaybackState
{
    private Director director;
    private AnimationManager animationManager;

    private const float SKIPPED_SPEED = 1.0f;
    private float targetTime;

    public RewindPlaybackState(Director director, AnimationManager animationManager, float targetTime)
    {
        this.director = director;
        this.animationManager = animationManager;
        this.targetTime = targetTime;

        animationManager.SetSpeed(/*SKIPPED_SPEED*/animationManager.LastSpeed);

        animationManager.SetAnimationDirection(-1.0f);
    }

    public void Update()
    {
        // Check ending condition.
        if (animationManager.CurrentTime <= targetTime)
        {
            animationManager.SetAnimationDirection(1.0f);
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
        // Do nothing.
    }

    public void Last()
    {
        // Do nothing.
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

