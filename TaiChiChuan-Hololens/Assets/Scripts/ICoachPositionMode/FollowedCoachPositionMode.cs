using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowedCoachPositionMode : ICoachPositionMode
{
    private Quaternion rotation;

    public FollowedCoachPositionMode()
    {
        rotation = Quaternion.identity;
    }

    public FollowedCoachPositionMode(Quaternion rotation)
    {
        this.rotation = rotation;
    }

    public void UpdateCoachPosition(Transform camera, float height, Transform avatars)
    {
        avatars.transform.position = camera.transform.position;
        avatars.transform.rotation = rotation;
        avatars.transform.Translate(Vector3.up * height);
    }

	public void UpdateClockPosition(Transform camera, float height, Transform clocks)
	{
		clocks.transform.position = camera.transform.position;
		clocks.transform.rotation = rotation;
		clocks.transform.Translate(Vector3.up * height);
	}

	public void SetRotation()
	{
		this.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
	}
}
