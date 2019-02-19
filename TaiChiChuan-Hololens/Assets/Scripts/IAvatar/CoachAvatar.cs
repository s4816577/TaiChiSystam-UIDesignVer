using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachAvatar : IAvatar
{
    private GameObject gameObject;
    private Transform coach;
    public Transform Avatar { get { return coach; } }

    private Transform head;
    public Transform Head { get { return head; } }
    private Transform leftFoot;
    public Transform LeftFoot { get { return leftFoot; } }
    private Transform rightFoot;
    public Transform RightFoot { get { return rightFoot; } }

	private List<float> restartErrorOfYdirection = new List<float> { -0.006f, -0.004f, -0.009f, -0.043f, -0.045f, -0.045f, -0.031f, -0.011f, -0.042f, -0.026f, -0.041f, -0.046f, -0.043f, -0.025f, -0.036f, -0.039f };

	public CoachAvatar(GameObject coachAvatar)
    {
        this.gameObject = coachAvatar;
        coach = this.gameObject.transform.Find("TaichiCoach");

        head = this.gameObject.transform.Find("TaichiCoach/Taiji_CC_Base_Root/Taiji_CC_Base_Hip/Taiji_CC_Base_Pelvis/Taiji_CC_Base_Waist/Taiji_CC_Base_Spine01/Taiji_CC_Base_Spine02/Taiji_CC_Base_NeckTwist01/Taiji_CC_Base_NeckTwist02/Taiji_CC_Base_Head");
        leftFoot = this.gameObject.transform.Find("TaichiCoach/Taiji_CC_Base_Root/Taiji_CC_Base_Hip/Taiji_CC_Base_Pelvis/Taiji_CC_Base_L_Thigh/Taiji_CC_Base_L_Calf/Taiji_CC_Base_L_Foot");
        rightFoot = this.gameObject.transform.Find("TaichiCoach/Taiji_CC_Base_Root/Taiji_CC_Base_Hip/Taiji_CC_Base_Pelvis/Taiji_CC_Base_R_Thigh/Taiji_CC_Base_R_Calf/Taiji_CC_Base_R_Foot");
    }

    public Quaternion HeadHorizontalRotation()
    {
        Quaternion rotation = head.rotation * Quaternion.Inverse(coach.rotation);
        Vector3 hozirontalRotation = rotation.eulerAngles;
        hozirontalRotation.x = hozirontalRotation.z = 0.0f;

        return Quaternion.Euler(hozirontalRotation);
    }

    /// <summary>
    /// Reset avatar's local rotation in order to make it face to the front.
    /// </summary>
    /// <param name="rotation">The rotation of avatar's head.</param>
    public void ResetRotation(Quaternion rotation)
    {
        coach.transform.localRotation = Quaternion.identity * Quaternion.Inverse(rotation);
    }

    /// <summary>
    /// Set avatar on its original local position.
    /// </summary>
    public void Update()
    {
		coach.localPosition = new Vector3(0.0f, coach.localPosition.y, 0.0f);
	}

	public void Reset(Transform clock, int restartInd)
	{
		coach.localPosition = new Vector3(0.0f, restartErrorOfYdirection[restartInd], 0.0f);
		coach.rotation = clock.rotation;
	}
}
