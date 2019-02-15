using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightControlPanel : MonoBehaviour
{
	protected const float DISTANT2CONTROLPANEL = 1.3f;
	private GameObject gaze;
	private bool moveToGaze = false;
	private float CoachHeight;

	// Use this for initialization
	protected void Start()
	{
		//get Coach Height from director
		GameObject tempDir = GameObject.Find("Director");
		Director tempScript = tempDir.GetComponent<Director>();
		CoachHeight = tempScript.LastAvatarsHeight;

		//set position
		transform.position = Camera.main.transform.position + DISTANT2CONTROLPANEL * Camera.main.transform.forward;
		transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y + CoachHeight, transform.position.z);
		//transform.forward = Camera.main.transform.forward;
		gaze = GameObject.Find("GazePoint");
		Debug.Log(gaze);
		gaze.transform.position = Camera.main.transform.position + DISTANT2CONTROLPANEL * Camera.main.transform.forward;
		gaze.transform.forward = Camera.main.transform.forward;
	}

	// Update is called once per frame
	protected void Update()
	{
		//set the y directional rotation 
		gaze.transform.position = Camera.main.transform.position + DISTANT2CONTROLPANEL * Camera.main.transform.forward;
		gaze.transform.forward = Camera.main.transform.forward;
		// Gaze too right, left, up, down
		if (gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > 15.0f ||
			gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < -15.0f)
		{
			moveToGaze = true;
		}

		if (gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y < 1.0f &&
			gaze.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y > -1.0f )
		{
			moveToGaze = false;
		}

		if (moveToGaze)
		{
			Vector3 currentPos = transform.position;
			Vector3 currentQuat = transform.rotation.eulerAngles;
			transform.position = Vector3.Lerp(transform.position, gaze.transform.position, 0.1f);
			transform.position = new Vector3(transform.position.x, currentPos.y, transform.position.z);
			transform.rotation = Quaternion.Lerp(transform.rotation, gaze.transform.rotation, 0.1f);
			transform.rotation = Quaternion.Euler(currentQuat.x, transform.rotation.eulerAngles.y, currentQuat.z);
		}

		//set y position
		transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y + CoachHeight, transform.position.z);
	}

	static public GameObject InstantiateGameObject()
	{
		GameObject g = Instantiate(ResourcePool.GetInstance().GetHeightControlPanelPrefab());

		//g.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

		return g;
	}

	public void SetCoachHeight(float height)
	{
		CoachHeight = height;
	}
}
