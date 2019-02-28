using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailedModeAnimationManager : AnimationManager 
{
	private int CountOfAudio = 0;

    private bool isAudioPlaying = false;
    private bool IsAudioPlaying
    {
        get
        {
            return isAudioPlaying;
        }
        set
        {
            if (value)
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
	private Queue<int> audioQueueInd = new Queue<int>();

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
		//no 8 stage 
		if (director.stageCode[director.stageCode.Count - 1] == 8)
		{ /*
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
			}*/
		}
		else if (director.stageCode[director.stageCode.Count - 1] == 1)
		{
			if (base.ClickPlaying)
			{
				if (!base.LockDuplicate)
				{
					if (lastMovementInd != base.currentMovementInd)
					{
						//CountOfAudio = 0;
						// Movement audio.
						audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].NumberSound);
						audioQueueInd.Enqueue(-2);
						audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].Sound);
						audioQueueInd.Enqueue(-1);
						// Action audio.
						if (canPlayActionAudio)
						{
							audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[base.currentActionInd].Sound);
							audioQueueInd.Enqueue(base.currentActionInd);
						}
					}
					else if (lastActionInd != base.currentActionInd)
					{
						// Action audio.
						if (canPlayActionAudio)
						{
							audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[currentActionInd].Sound);
							audioQueueInd.Enqueue(currentActionInd);
						}
					}
				}

				// If no audio is playing and we still have audio in queue.
				if (!audioSource.isPlaying && audioQueue.Count != 0)
				{
					if(base.lastSpeed > 1.0f)
						audioSource.pitch = base.lastSpeed;
					else
						audioSource.pitch = 1.0f;

					audioSource.PlayOneShot(audioQueue.Dequeue());
					int currentAudioInd = audioQueueInd.Dequeue();
					Debug.Log(currentActionInd);
					if (currentAudioInd == -2 || currentAudioInd == -1)
						IsAudioPlaying = true;
					else if (currentAudioInd >= 0)
					{
						IsAudioPlaying = false;
						LockDuplicate = false;
					}
					//CountOfAudio++;
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
					if (!base.LockDuplicate)
					{
						if (base.currentActionInd == 0)
						{
							//CountOfAudio = 0;
							audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].NumberSound);
							audioQueueInd.Enqueue(-2);
							audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].Sound);
							audioQueueInd.Enqueue(-1);
						}
						if (canPlayActionAudio)
						{
							audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[currentActionInd].Sound);
							audioQueueInd.Enqueue(base.currentActionInd);
						}
					}
				}
				/*
				if (CountOfAudio == 0)
					IsAudioPlaying = true;*/

				if (!audioSource.isPlaying && audioQueue.Count != 0)
				{
					//CountOfAudio++;
					if (base.lastSpeed > 1.0f)
						audioSource.pitch = base.lastSpeed;
					else
						audioSource.pitch = 1.0f;

					audioSource.PlayOneShot(audioQueue.Dequeue());
					int currentAudioInd = audioQueueInd.Dequeue();
					if (currentAudioInd == -2 || currentAudioInd == -1)
						IsAudioPlaying = true;
					else if (currentAudioInd >= 0)
					{
						IsAudioPlaying = false;
						LockDuplicate = false;
					}
					//IsAudioPlaying = true;
				}
				// If the audioSource just stop playing audio.
				else if (!audioSource.isPlaying && IsAudioPlaying)
				{
					//IsAudioPlaying = false;
				}
				/*
				if (CountOfAudio == 3)
				{
					IsAudioPlaying = false;
					LockDuplicate = false;
				}*/
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
		ClearAudio();
		// Movement audio.
		//CountOfAudio = 0;
		if (base.currentActionInd == 0)
		{
			audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].NumberSound);
			audioQueueInd.Enqueue(-2);
			audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].Sound);
			audioQueueInd.Enqueue(-1);
		}
		// Action audio.
		audioQueue.Enqueue(taichiMovementArray[base.currentMovementInd].TaichiActionArray[base.currentActionInd].Sound);
		audioQueueInd.Enqueue(base.currentActionInd);
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
		audioQueueInd.Clear();
	}

	public override void PlaySoundInd(int Ind)
	{
		ClearAudio();
		// Movement audio.
		//CountOfAudio = 0;
		audioQueue.Enqueue(taichiMovementArray[Ind].NumberSound);
		audioQueueInd.Enqueue(-2);
		audioQueue.Enqueue(taichiMovementArray[Ind].Sound);
		audioQueueInd.Enqueue(-1);
		// Action audio.
		audioQueue.Enqueue(taichiMovementArray[Ind].TaichiActionArray[0].Sound);
		audioQueueInd.Enqueue(0);
	}

	public override void Replay()
	{
		if (IsAudioPlaying)
		{
			base.ExecuteLastMovement();
			base.PlayAfterLast = true;
		}
		else
		{
			base.ReplayToCurrentMovementStart();
			base.PlayAfterLast = true;
		}
	}
}
