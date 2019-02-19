using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachFreezeAtStart : MonoBehaviour {

	protected List<TaichiMovement> taichiMovementArray;
	private Animator anim;
	private int restartInd = 3;

	// Use this for initialization
	void Start()
	{
		//get anim
		anim = this.transform.GetComponent<Animator>();

		//get movement array and set the playing start point
		taichiMovementArray = ResourcePool.GetInstance().TaichiMovementArray;
		anim.Play("Animation", 0, taichiMovementArray[restartInd].NormalizedBeginTime);

		//set init speed
		anim.speed = 0.0f;

		//adjust y
		this.transform.localPosition = new Vector3(this.transform.localPosition.x, -0.043f, this.transform.localPosition.z);
	}
}
