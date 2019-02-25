using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AnimationManager
{
    // Constant
    private const float DEFAULT_SPEED = 1.0f;

    // Taichi movement & action.
    public List<TaichiMovement> taichiMovementArray;
    private int lastMovementInd;
    public int currentMovementInd;
    private int lastActionInd;
    public int currentActionInd;
	public int restartInd = 3;
	private bool LastActionPause = true;
	private bool LastMovementPause = true;
	private StageMode stageMode;

	protected AvatarsController avatarsController;
	protected Director director;
    private List<Animator> animators = new List<Animator>();
    public AudioSource audioSource;

    // Delagates.
    private AnimationDelegator animationDelegator;
    public AnimationDelegator AnimationDelegator { get { return animationDelegator; } }

	// TODO Playback State.
	// protected IPlaybackState playbackState;
	// TODO
	public bool IsSkippingByNextLast = false;
    public bool shouldPlaying = false;
	public bool ClickPlaying = false;
    protected float speed;
    protected float lastSpeed = DEFAULT_SPEED;
    public float LastSpeed
	{
		get
		{
			return lastSpeed;
		}
		set
		{
			lastSpeed = value;
		}
	}

    // Record the current time of animation, presented in 0.0~1.0 .
    private float currentTime;
    public float CurrentTime
    {
        get
        {
            return currentTime;
        }
        set
        {
            currentTime = value;
        }
    }


    public AnimationManager(Director director, List<IAvatar> avatars, AudioSource audioSource, AvatarsController avatarsController)
	{
        // Taichi movement.
        taichiMovementArray = ResourcePool.GetInstance().TaichiMovementArray;
        lastMovementInd = -1;
        currentMovementInd = 0;
        // Taichi action.
        lastActionInd = -1;
        currentActionInd = 0;

        this.director = director;
        foreach (IAvatar avatar in avatars)
        {
            animators.Add(avatar.Avatar.GetComponent<Animator>());
            animators[animators.Count - 1].speed = 0.0f;
        }
        this.audioSource = audioSource;
		this.avatarsController = avatarsController;

		stageMode = GameObject.Find("Stage").GetComponent<StageMode>();
        animationDelegator = new AnimationDelegator();
    }

    public virtual void Update()
    {
        UpdateAndCheckIndex();

        // If currentTime does not update by PlaybackState during state changing, we should calculate currentTime.
        // Get current time using first animator.
        Animator anmt = animators[1];
        AnimatorStateInfo animatorState = anmt.GetCurrentAnimatorStateInfo(0);
        currentTime = animatorState.normalizedTime;
    }

    public virtual void UpdateAndCheckIndex()
    {
        UpdateMovement();
        UpdateAction();
    }

    private void UpdateMovement()
    {		
		TaichiMovement currentMovement = taichiMovementArray[currentMovementInd];

        // Current movement is correct, do nothing.
        if (lastMovementInd != -1 &&
            currentMovement.NormalizedBeginTime <= currentTime &&
            currentTime < currentMovement.NormalizedEndTime)
            return;

        // Current movement is not correct, and it's faster than the current time.
        else if (currentMovement.NormalizedBeginTime > currentTime)
        {
            // Repeat go backward until correct movement, also check boundary. 
            while (taichiMovementArray[currentMovementInd].NormalizedBeginTime > currentTime && currentMovementInd > 0)
            {
                currentMovementInd--;
            }
        }
        // Current movement is not correct, and it's slower than the current time.
        else if (currentTime >= currentMovement.NormalizedEndTime)
        {
            // Repeat go forward until correct movement, also check boundary. 
            while (currentTime >= taichiMovementArray[currentMovementInd].NormalizedEndTime && currentMovementInd < taichiMovementArray.Count - 1)
            {
                currentMovementInd++;
            }
        }

        lastActionInd = -1;
        currentActionInd = 0;
		
		// Check Movement name changed.
		if (lastMovementInd != currentMovementInd)
        {
			if (currentMovementInd != restartInd && director.singleMode && !IsSkippingByNextLast)
			{
				currentMovementInd = restartInd;
				ExecuteRestart();
				if (ClickPlaying)
					ClickPlaying = false;
			}
            lastMovementInd = currentMovementInd;
            currentMovement = taichiMovementArray[currentMovementInd];
            //animationDelegator.InvokeMovementChange(currentMovement.MovementNumber.ToString() + ". " + currentMovement.MovementName);
			stageMode.MovementName = currentMovement.MovementName;
			if (IsSkippingByNextLast)
			{
				restartInd = currentMovementInd;
				IsSkippingByNextLast = false;
			}
        }

		//remove t pose at the end
		if (currentTime >= taichiMovementArray[15].NormalizedEndTime)
		{
			if (director.stageCode[director.stageCode.Count - 1] == 1)
			{
				currentMovementInd = 0;
				restartInd = 0;
				ExecuteRestart();
				if (ClickPlaying)
					ClickPlaying = false;
				lastMovementInd = currentMovementInd;
				currentMovement = taichiMovementArray[currentMovementInd];
				//animationDelegator.InvokeMovementChange(currentMovement.MovementNumber.ToString() + ". " + currentMovement.MovementName);
				stageMode.MovementName = currentMovement.MovementName;
			}
			else if (director.stageCode[director.stageCode.Count - 1] == 2)
			{
				restartInd = 15;
				currentMovementInd = restartInd;
				ExecuteRestart();
				if (ClickPlaying)
					ClickPlaying = false;
				lastMovementInd = currentMovementInd;
				currentMovement = taichiMovementArray[currentMovementInd];
				//animationDelegator.InvokeMovementChange(currentMovement.MovementNumber.ToString() + ". " + currentMovement.MovementName);
				stageMode.MovementName = currentMovement.MovementName;
			}
		}
	}

    private void UpdateAction()
    {
        TaichiMovement currentMovement = taichiMovementArray[currentMovementInd];
        List<TaichiAction> taichiActionArray = currentMovement.TaichiActionArray;
        TaichiAction currentAction = taichiActionArray[currentActionInd];

        // Current action is correct, do nothing.
        if (lastActionInd != -1 &&
            currentAction.NormalizedBeginTime <= currentTime 
            && currentTime < currentAction.NormalizedEndTime)
            return;

        // Current action is not correct, and it's faster than the current time.
        else if (currentAction.NormalizedBeginTime > currentTime)
        {
            // Repeat go backward until correct action, also check boundary. 
            while (taichiActionArray[currentActionInd].NormalizedBeginTime > currentTime && currentActionInd > 0)
            {
                currentActionInd--;
            }
        }
        // Current action is not correct, and it's slower than the current time.
        else if (currentTime >= currentAction.NormalizedEndTime)
        {
            // Repeat go forward until correct action, also check boundary. 
            while (currentTime >= taichiActionArray[currentActionInd].NormalizedEndTime && currentActionInd < taichiActionArray.Count - 1)
            {
                currentActionInd++;
            }
        }


        // Check Action name changed.
        if (lastActionInd != currentActionInd)
        {
            lastActionInd = currentActionInd;
            currentAction = taichiActionArray[currentActionInd];
            //animationDelegator.InvokeActionChange(currentAction.ActionName.ToString());
			stageMode.ActionName = currentAction.ActionName.ToString();
        }
    }
    
    public abstract void PlaySound();

    public void SetSpeed(float speed)
    {
        foreach (Animator anim in animators)
        {
            anim.speed = speed;
        }
        this.speed = speed;
    }

    public void SetAnimationDirection(float direction)
    {
        foreach (Animator anim in animators)
        {
            if (direction > 0)
                anim.SetFloat("direction", 1.0f);
            else
                anim.SetFloat("direction", -1.0f);
        }
    }

    public void SetAnimationTime(float normalizeTime)
    {
        foreach (Animator anim in animators)
        {
            anim.Play("Animation", 0, normalizeTime);
        }
        SetSpeed(0.0f);
    }

    public void ResetLastSpeed()
    {
        lastSpeed = speed;
    }

    public void ExecuteRestart()
    {
        foreach (Animator anim in animators)
        {
            anim.Play("Animation", 0, taichiMovementArray[restartInd].NormalizedBeginTime);
        }
        SetSpeed(0.0f);
		director.NotShowingPauseLog();
		director.Pause();
		avatarsController.ResetAvatersPosition(restartInd);
		ClearAudio();
	}

    public void ExecuteNextMovement()
    {
        if (currentMovementInd + 1 != taichiMovementArray.Count)
        {
            int newMovementInd = currentMovementInd + 1;
            TaichiMovement newMovement = taichiMovementArray[newMovementInd];
            director.SetPlaybackState(new FastforwardPlayback(director, this, newMovement.NormalizedBeginTime));
        }
		else if (currentMovementInd + 1 == taichiMovementArray.Count)
		{
			if (LastMovementPause)
			{
				int newMovementInd = 15;
				TaichiMovement newMovement = taichiMovementArray[newMovementInd];
				director.SetPlaybackState(new FastforwardPlayback(director, this, newMovement.NormalizedEndTime - 0.0001f));
				LastMovementPause = false;
			}
			else
			{
				currentMovementInd = 0;
				restartInd = 0;
				ExecuteRestart();
				LastMovementPause = true;
			}
		}
	}

    public void ExecuteLastMovement()
    {
		if (currentMovementInd != 0)
		{
			int newMovementInd = currentMovementInd - 1;
			TaichiMovement newMovement = taichiMovementArray[newMovementInd];
			director.SetPlaybackState(new RewindPlaybackState(director, this, newMovement.NormalizedBeginTime));
		}
		else if (currentMovementInd == 0)
		{
			int newMovementInd = 0;
			TaichiMovement newMovement = taichiMovementArray[newMovementInd];
			director.SetPlaybackState(new RewindPlaybackState(director, this, newMovement.NormalizedBeginTime));
		}
    }

	public virtual void ExecuteNextAction()
	{
		if (director.seriesMode)
		{
			if (currentActionInd + 1 != taichiMovementArray[currentMovementInd].TaichiActionArray.Count)
			{
				int newActionInd = currentActionInd + 1;
				TaichiAction newAction = taichiMovementArray[currentMovementInd].TaichiActionArray[newActionInd];
				director.SetPlaybackState(new FastforwardPlayback(director, this, newAction.NormalizedBeginTime));
			}
			else if (currentMovementInd + 1 != taichiMovementArray.Count)
			{
				int newMovementInd = currentMovementInd + 1;
				List<TaichiAction> taichiActionArray = taichiMovementArray[newMovementInd].TaichiActionArray;
				TaichiAction newAction = taichiActionArray[0];
				director.SetPlaybackState(new FastforwardPlayback(director, this, newAction.NormalizedBeginTime));
			}
		}
		else if (director.singleMode)
		{
			if (currentActionInd + 1 != taichiMovementArray[currentMovementInd].TaichiActionArray.Count)
			{
				int newActionInd = currentActionInd + 1;
				TaichiAction newAction = taichiMovementArray[currentMovementInd].TaichiActionArray[newActionInd];
				director.SetPlaybackState(new FastforwardPlayback(director, this, newAction.NormalizedBeginTime));
			}
			else if (currentActionInd + 1 == taichiMovementArray[currentMovementInd].TaichiActionArray.Count)
			{
				if (currentMovementInd == 15)
				{
					//SetSpeed(1.0f);
					if (LastActionPause)
					{
						director.SetPlaybackState(new FastforwardPlayback(director, this, taichiMovementArray[15].NormalizedEndTime - 0.0001f));
						LastActionPause = false;
					}
					else
					{
						ExecuteRestart();
						LastActionPause = true;
					}
				}
				else
				{
					//SetSpeed(1.0f);
					if (LastActionPause)
					{
						int newActionInd = 0;
						TaichiAction newAction = taichiMovementArray[currentMovementInd + 1].TaichiActionArray[newActionInd];
						director.SetPlaybackState(new FastforwardPlayback(director, this, newAction.NormalizedBeginTime - 0.0001f));
						LastActionPause = false;
					}
					else
					{
						ExecuteRestart();
						LastActionPause = true;
					}
				}
			}
		}
		//else if (currentMovementInd + 1 != taichiMovementArray.Count)
		//{
		//    int newMovementInd = currentMovementInd + 1;
		//    List<TaichiAction> taichiActionArray = taichiMovementArray[newMovementInd].TaichiActionArray;
		//    TaichiAction newAction = taichiActionArray[0];
		//    director.SetPlaybackState(new FastforwardPlayback(director, this, newAction.NormalizedBeginTime));
		//}
	}

    public virtual void ExecuteLastAction()
    {
        if (currentActionInd != 0)
        {
            int newActionInd = currentActionInd - 1;
            TaichiAction newAction = taichiMovementArray[currentMovementInd].TaichiActionArray[newActionInd];
            director.SetPlaybackState(new RewindPlaybackState(director, this, newAction.NormalizedBeginTime));
        }
        else if (currentMovementInd != 0 && director.seriesMode)
        {
            int newMovementInd = currentMovementInd - 1;
            List<TaichiAction> taichiActionArray = taichiMovementArray[newMovementInd].TaichiActionArray;
            TaichiAction newAction = taichiActionArray[taichiActionArray.Count - 1];
            director.SetPlaybackState(new RewindPlaybackState(director, this, newAction.NormalizedBeginTime));
        }
    }

    public abstract void ExecuteNext();

    public abstract void ExecuteLast();

	public abstract void PlaySegmentedSound();

	public abstract void ClearAudio();

	public void UpdateFirstCoachAnamator(Animator firstCoachAnimator)
    {
        animators[0] = firstCoachAnimator;
        animators[0].speed = speed;
        animators[0].Play("Animation", 0, currentTime);
    }

	public void SetReatartInd(int Ind)
	{
		restartInd = Ind;
	}
}
