using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousModeAnimationManager : AnimationManager 
{
    public ContinuousModeAnimationManager(Director director, List<IAvatar> avatars, AudioSource audioSource, AvatarsController avatarsController)
        : base(director, avatars, audioSource, avatarsController)
    {
	}

    public override void UpdateAndCheckIndex()
    {
        int lastMovementInd = base.currentMovementInd;
        base.UpdateAndCheckIndex();

        if (lastMovementInd != base.currentMovementInd)
        {
            audioSource.PlayOneShot(taichiMovementArray[base.currentMovementInd].Sound);
        }
    }

    public override void PlaySound()
    {
        audioSource.PlayOneShot(taichiMovementArray[currentMovementInd].Sound);
    }

    public override void ExecuteNext()
    {
        base.ExecuteNextMovement();
    }

    public override void ExecuteLast()
    {
        base.ExecuteLastMovement();
    }

    public override void ExecuteNextAction()
    {
        // Do nothing.
    }

    public override void ExecuteLastAction()
    {
        // Do nothing.
    }

	public override void PlaySegmentedSound()
	{
		//Do nothing
	}

	public override void ClearAudio()
	{
		//Do nothing
	}

	public override void PlaySoundInd(int Ind)
	{
		//Do nothing
	}

	public override void Replay()
	{
		//Do nothing
	}
}
