﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlPanel : MonoBehaviour
{
	protected const float DISTANT2CONTROLPANEL = 1.0f;
	private GameObject gaze;
	private bool moveToGaze = false;

	protected virtual void Awake()
	{
	}

	// Use this for initialization
	protected virtual void Start()
	{
		transform.position = Camera.main.transform.position + DISTANT2CONTROLPANEL * Camera.main.transform.forward;
		transform.forward = Camera.main.transform.forward;
		gaze = GameObject.Find("GazePoint");
		Debug.Log(gaze);
		gaze.transform.position = Camera.main.transform.position + DISTANT2CONTROLPANEL * Camera.main.transform.forward;
		gaze.transform.forward = Camera.main.transform.forward;
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		gaze.transform.position = Camera.main.transform.position + DISTANT2CONTROLPANEL * Camera.main.transform.forward;
		gaze.transform.forward = Camera.main.transform.forward;
		// Gaze too right, left, up, down
		/*
		if (gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > 18.0f ||
			gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < -18.0f ||
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x > 8.0f ||
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x < -8.0f)
		{
			moveToGaze = true;
		}

		if (gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < 6.0f &&
			gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > -6.0f &&
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x < 6.0f &&
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x > -6.0f)
		{
			moveToGaze = false;
		}

		if (moveToGaze)
		{
			transform.position = Vector3.Lerp(transform.position, gaze.transform.position, 0.07f);
			transform.rotation = Quaternion.Lerp(transform.rotation, gaze.transform.rotation, 0.07f);
		}*/

		RaycastHit hitInfo;
		if (Physics.Raycast(
				Camera.main.transform.position,
				Camera.main.transform.forward,
				out hitInfo,
				20.0f,
				Physics.DefaultRaycastLayers) == false)
		{
			moveToGaze = true;
		}

		if (gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < 6.0f &&
			gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > -6.0f &&
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x < 6.0f &&
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x > -6.0f)
		{
			moveToGaze = false;
		}

		if (moveToGaze)
		{
			transform.position = Vector3.Lerp(transform.position, gaze.transform.position, 0.035f);
			transform.rotation = Quaternion.Lerp(transform.rotation, gaze.transform.rotation, 0.035f);
		}

		/*
		if (gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > 18.0f ||
			gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < -18.0f ||
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x > 8.0f ||
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x < -8.0f)
		{
			transform.position = Vector3.Lerp(transform.position, gaze.transform.position, 0.03f);
			transform.rotation = Quaternion.Lerp(transform.rotation, gaze.transform.rotation, 0.03f);
		}*/
	}

	/*
	void Transfer()
	{
		if (!(gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < 6.0f &&
			   gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > -6.0f &&
			   gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x < 6.0f &&
			   gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x > -6.0f))
		{
			transform.position = Vector3.Lerp(transform.position, gaze.transform.position, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, gaze.transform.rotation, 0.1f);
		}
	}*/
}
