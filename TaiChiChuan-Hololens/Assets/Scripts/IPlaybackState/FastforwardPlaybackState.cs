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

	private Queue<AudioClip> audioQueue = new Queue<AudioClip>();

	public FastforwardPlayback(Director director, AnimationManager animationManager, float targetTime)
    {
        this.director = director;
        this.animationManager = animationManager;
        this.targetTime = targetTime;

		//set speed in different mode
		float SetSpeed;
		if (director.stageCode[director.stageCode.Count - 1] == 1 || director.stageCode[director.stageCode.Count - 1] == 2)
			SetSpeed = 8.0f;
		else
			SetSpeed = animationManager.LastSpeed;

		animationManager.SetSpeed(/*SKIPPED_SPEED*/SetSpeed);

		//set skip loop in mode 2
		if (director.stageCode[director.stageCode.Count - 1] == 2)
		{
			animationManager.IsSkippingByNextLast = true;
		}

		if (director.stageCode[director.stageCode.Count - 1] == 1)
		{
			animationManager.ClearAudio();
			animationManager.ClickPlaying = false;
		}


		if (director.stageCode[director.stageCode.Count - 1] == 3)
			animationManager.PlaySegmentedSound();
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
		/*
		animationManager.ExecuteLast();

		// Show "Pause" icon on UI.
		animationManager.AnimationDelegator.InvokePlayIcon(false);*/
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
		if (director.stageCode[director.stageCode.Count - 1] == 1 || director.stageCode[director.stageCode.Count - 1] == 2)
			return false;
		else
			return true;
	}

	public void SetRestartInd(int Ind)
	{
		animationManager.SetReatartInd(Ind);
		animationManager.ExecuteRestart();
		animationManager.AnimationDelegator.InvokePlayIcon(false);
		director.SetPlaybackState(new NormalPlaybackState(director, animationManager));
	}

	public void PlayAndSound(int Ind)
	{	/*
		animationManager.PlaySound();
		animationManager.SetSpeed(animationManager.LastSpeed);
		animationManager.shouldPlaying = true;
		animationManager.ClickPlaying = true;*/
	}
}

