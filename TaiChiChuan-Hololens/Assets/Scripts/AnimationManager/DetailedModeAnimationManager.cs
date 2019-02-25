using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailedModeAnimationManager : AnimationManager 
{
    private bool isAudioPlaying = false;
    private bool IsAudioPlaying
    {
        get
        {
            return isAudioPlaying;
        }
        set
        {
            if (value && (base.director.stageCode[base.director.stageCode.Count - 1] == 3))
            {
                if (base.shouldPlaying)
                {
                    base.SetSpeed(0.0f);
                }
            }
            else
            {
                if (base.shouldPlaying)
                {
                    base.SetSpeed(base.lastSpeed);
                }
            }
            isAudioPlaying = value;
        }
    }

    private Queue<AudioClip> audioQueue = new Queue<AudioClip>();

    public DetailedModeAnimationManager(Director director, List<IAvatar> avatars, AudioSource audioSource, AvatarsController avatarsController)
        : base(director, avatars, audioSource, avatarsController)
    {
    }

    public override void UpdateAndCheckIndex()
    {
        int lastMovementInd = base.currentMovementInd;
        int lastActionInd = base.currentActionInd;
        bool canPlayActionAudio = base.director.PlaybackState.CanPlayActionAudio();
        base.UpdateAndCheckIndex();

		/*if (base.director.stageCode[base.director.stageCode.Count - 1] != 3)
			canPlayActionAudio = false;*/
		if (director.stageCode[director.stageCode.Count - 1] == 8)
		{
			if (lastMovementInd != base.currentMovementInd)
			{
				// Movement audio.
				//audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].Sound);
				// Action audio.
				//if (canPlayActionAudio)
				//	audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[base.currentActionInd].Sound);
			}
			else if (lastActionInd != base.currentActionInd)
			{
				// Action audio.
				if (canPlayActionAudio)
					audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[currentActionInd].Sound);
			}

			// If no audio is playing and we still have audio in queue.
			if (!audioSource.isPlaying && audioQueue.Count != 0)
			{
				audioSource.PlayOneShot(audioQueue.Dequeue());
				//IsAudioPlaying = true;
			}
			// If the audioSource just stop playing audio.
			else if (!audioSource.isPlaying && IsAudioPlaying)
			{
				//IsAudioPlaying = false;
			}
		}
		else if (director.stageCode[director.stageCode.Count - 1] == 1)
		{
			if (base.ClickPlaying)
			{
				if (lastMovementInd != base.currentMovementInd)
				{
					// Movement audio.
					audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].Sound);
					// Action audio.
					if (canPlayActionAudio)
						audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[base.currentActionInd].Sound);
				}
				else if (lastActionInd != base.currentActionInd)
				{
					// Action audio.
				if (canPlayActionAudio)
					audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[currentActionInd].Sound);
				}

				// If no audio is playing and we still have audio in queue.
				if (!audioSource.isPlaying && audioQueue.Count != 0)
				{
					audioSource.PlayOneShot(audioQueue.Dequeue());
					//IsAudioPlaying = true;
				}
				// If the audioSource just stop playing audio.
				else if (!audioSource.isPlaying && IsAudioPlaying)
				{
					//IsAudioPlaying = false;
				}
			}
		}
		else if (director.stageCode[director.stageCode.Count - 1] == 2)
		{
			if (base.ClickPlaying)
			{
				if (lastMovementInd != base.currentMovementInd)
				{
					// Movement audio.
					//audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].Sound);
					// Action audio.
					//if (canPlayActionAudio)
					//	audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[base.currentActionInd].Sound);
				}
				else if (lastActionInd != base.currentActionInd)
				{
					// Action audio.
					if (canPlayActionAudio)
						audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[currentActionInd].Sound);
				}

				if (!audioSource.isPlaying && audioQueue.Count != 0)
				{
					audioSource.PlayOneShot(audioQueue.Dequeue());
					//IsAudioPlaying = true;
				}
				// If the audioSource just stop playing audio.
				else if (!audioSource.isPlaying && IsAudioPlaying)
				{
					//IsAudioPlaying = false;
				}
			}
		}
		else
		{

			//Do nothing
			if (!audioSource.isPlaying && audioQueue.Count != 0)
			{
				audioSource.PlayOneShot(audioQueue.Dequeue());
				//IsAudioPlaying = true;
			}
			// If the audioSource just stop playing audio.
			else if (!audioSource.isPlaying && IsAudioPlaying)
			{
				//IsAudioPlaying = false;
			}
		}
    }

    public override void PlaySound()
    {
        // Movement audio.
        audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].Sound);
        // Action audio.
        audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[base.currentActionInd].Sound);
    }

    public override void ExecuteNext()
    {
		if (base.director.stageCode[base.director.stageCode.Count - 1] == 3)
			base.ExecuteNextAction();
		else
			base.ExecuteNextMovement();
    }

    public override void ExecuteLast()
    {
		if (base.director.stageCode[base.director.stageCode.Count - 1] == 3)
			base.ExecuteLastAction();
		else
			base.ExecuteLastMovement();
	}

	public override void PlaySegmentedSound()
	{
		// Movement audio.
		if (currentActionInd == 0)
			audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].Sound);
		// Action audio.
		audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[base.currentActionInd].Sound);
	}

	public override void ClearAudio()
	{
		audioQueue.Clear();
	}
}
