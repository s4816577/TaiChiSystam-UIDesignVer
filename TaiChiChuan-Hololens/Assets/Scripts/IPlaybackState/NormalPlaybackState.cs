using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlaybackState : IPlaybackState
{
    private Director director;
    private AnimationManager animationManager;

    private const float MAX_SPEED = 4.0f;
    private const float MIN_SPEED = 0.25f;

    public NormalPlaybackState(Director director, AnimationManager animationManager)
    {
        this.director = director;
        this.animationManager = animationManager;
        Pause();
    }

    public void Update()
    {

    }

    /// <summary>
    /// Play all animators in list "avatars" with original speed.
    /// </summary>
    public void Play()
    {
        animationManager.PlaySound();
        animationManager.SetSpeed(animationManager.LastSpeed);
        animationManager.shouldPlaying = true;
		animationManager.ClickPlaying = true;

        // Show "Play" icon on UI.
        animationManager.AnimationDelegator.InvokePlayIcon(true);
    }

    /// <summary>
    /// Stop playing all animators in list "avatars".
    /// </summary>
    public void Pause()
    {
        animationManager.SetSpeed(0.0f);
        animationManager.shouldPlaying = false;
		animationManager.ClickPlaying = false;
		animationManager.ClearAudio();

        // Show "Pause" icon on UI.
        animationManager.AnimationDelegator.InvokePlayIcon(false);
    }

    public void Restart()
    {
        animationManager.ExecuteRestart();
        Pause();
    }

    /// <summary>
    /// Increase the playing speed of all animators by 2.0 times.
    /// </summary>
    public void SpeedUp()
    {
        if (animationManager.LastSpeed * 2.0f > MAX_SPEED)
            return;
        animationManager.SetSpeed(animationManager.LastSpeed * 2.0f);
        animationManager.ResetLastSpeed();
        Play();

        // Update speed display on UI.
        animationManager.AnimationDelegator.InvokeSpeedValueChange(animationManager.LastSpeed);
    }

    /// <summary>
    /// Decrease the playing speed of all animators by 0.5 times.
    /// </summary>
    public void SpeedDown()
    {
        if (animationManager.LastSpeed * 0.5f < MIN_SPEED)
            return;
        animationManager.SetSpeed(animationManager.LastSpeed * 0.5f);
        animationManager.ResetLastSpeed();
        Play();

        // Update speed display on UI.
        animationManager.AnimationDelegator.InvokeSpeedValueChange(animationManager.LastSpeed);
    }

    public void Next()
    {
        animationManager.ExecuteNext();

        // Show "Pause" icon on UI.
        animationManager.AnimationDelegator.InvokePlayIcon(false);
    }

    public void Last()
    {
        animationManager.ExecuteLast();

        // Show "Pause" icon on UI.
        animationManager.AnimationDelegator.InvokePlayIcon(false);
    }

    public void NextMovement()
    {
        animationManager.ExecuteNextMovement();

        // Show "Pause" icon on UI.
        animationManager.AnimationDelegator.InvokePlayIcon(false);
    }

    public void LastMovement()
    {
        animationManager.ExecuteLastMovement();

        // Show "Pause" icon on UI.
        animationManager.AnimationDelegator.InvokePlayIcon(false);
    }

    public void NextAction()
    {
        animationManager.ExecuteNextAction();

        // Show "Pause" icon on UI.
        animationManager.AnimationDelegator.InvokePlayIcon(false);
    }

    public void LastAction()
    {
        animationManager.ExecuteLastAction();

        // Show "Pause" icon on UI.
        animationManager.AnimationDelegator.InvokePlayIcon(false);
    }

    public bool CanPlayActionAudio()
    {
        return true;
    }

	public void SetRestartInd(int Ind)
	{
		animationManager.SetReatartInd(Ind);
		animationManager.ExecuteRestart();
		animationManager.AnimationDelegator.InvokePlayIcon(false);
	}
}
