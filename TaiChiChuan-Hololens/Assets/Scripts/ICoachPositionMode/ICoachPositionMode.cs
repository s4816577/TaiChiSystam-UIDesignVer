using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoachPositionMode
{
    void UpdateCoachPosition(Transform camera, float height, Transform avatars);
	void UpdateClockPosition(Transform camera, float height, Transform avatars);
}
