using System.Collections;
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
		if (gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > 15.0f ||
			gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < -15.0f ||
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x > 8.0f ||
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x < -8.0f)
		{
			moveToGaze = true;
		}

		if (gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < 1.0f &&
			gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > -1.0f &&
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x < 1.0f &&
			gaze.transform.rotation.eulerAngles.x - transform.rotation.eulerAngles.x > -1.0f)
		{
			moveToGaze = false;
		}

		if (moveToGaze)
		{
			transform.position = Vector3.Lerp(transform.position, gaze.transform.position, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, gaze.transform.rotation, 0.1f);
		}
	}
}
