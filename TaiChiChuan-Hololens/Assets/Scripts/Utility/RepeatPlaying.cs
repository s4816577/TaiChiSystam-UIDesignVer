using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatPlaying : MonoBehaviour {

	protected List<TaichiMovement> taichiMovementArray;
	private Animator anim;
	//private Quaternion initQuaternion;
	private float LastPlayingSpeed = 1.0f;
	private float CurrentPlayingSpeed = 1.0f;
	private float currentTime;
	private int restartInd = 3;

	// Use this for initialization
	void Start () {
		//get anim
		anim = this.transform.GetComponent<Animator>();

		//get movement array and set the playing start point
		taichiMovementArray = ResourcePool.GetInstance().TaichiMovementArray;
		anim.Play("Animation", 0, taichiMovementArray[restartInd].NormalizedBeginTime);

		//set init speed
		anim.speed = CurrentPlayingSpeed;

		//get init rotation
		//initQuaternion = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		//get the time of anim
		AnimatorStateInfo animatorState = anim.GetCurrentAnimatorStateInfo(0);
		currentTime = animatorState.normalizedTime;

		//update movement and speed if wrong
		UpdateSpeed();
		UpdateMovement();
	}

	private void UpdateMovement()
	{
		//find the currentMovementInd

		int currentMovementInd = 0;
		TaichiMovement currentMovement = taichiMovementArray[currentMovementInd];

		// Current movement is correct, do nothing.
		if (currentMovement.NormalizedBeginTime <= currentTime &&
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

		// Check if Movement is restartInd.
		if (currentMovementInd != restartInd)
		{
			anim.Play("Animation", 0, taichiMovementArray[restartInd].NormalizedBeginTime);
			anim.speed = CurrentPlayingSpeed;
			Reset();
		}
			
	}

	private void UpdateSpeed()
	{
		if (CurrentPlayingSpeed != LastPlayingSpeed)
		{
			anim.speed = CurrentPlayingSpeed;
			LastPlayingSpeed = CurrentPlayingSpeed;
			//Reset();
		}
	}

	private void Reset()
	{
		this.transform.localPosition = Vector3.zero;
		this.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
	}

	public void SetPlayingSpeed(float Speed)
	{
		CurrentPlayingSpeed = Speed;
	}
}
