using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachAssistantAvatar : IAvatar
{
    static public readonly string TA_AVATAR_NAME = "TaichiTA";

    private GameObject gameObject;
    private Transform teachAssistant;
    public Transform Avatar { get { return teachAssistant; } }

	private Transform leftFoot;
	public Transform LeftFoot { get { return leftFoot; } }
	private Transform rightFoot;
	public Transform RightFoot { get { return rightFoot; } }

	private List<float> restartErrorOfYdirection = new List<float> { -0.006f, -0.004f, -0.009f, -0.043f, -0.045f, -0.045f, -0.031f, -0.011f, -0.042f, -0.026f, -0.041f, -0.046f, -0.043f, -0.025f, -0.036f, -0.039f };

	public TeachAssistantAvatar(GameObject teachAssistantAvatar)
    {
        this.gameObject = teachAssistantAvatar;
        teachAssistant = this.gameObject.transform.Find(TeachAssistantAvatar.TA_AVATAR_NAME);

		leftFoot = this.gameObject.transform.Find("TaichiTA/SH_ROOTJ/SH_ROOT02J/SH_lHipJ/SH_lKneeJ/SH_lAnkleJ");
		rightFoot = this.gameObject.transform.Find("TaichiTA/SH_ROOTJ/SH_ROOT02J/SH_rHipJ/SH_rKneeJ/SH_rAnkleJ");
		
		//Debug.Log(leftFoot);
	}

	public void ResetRotation(Quaternion rotation)
    {
        teachAssistant.transform.localRotation = Quaternion.identity * Quaternion.Inverse(rotation);
    }

    public void Update()
    {
		teachAssistant.localPosition = new Vector3(0.0f, teachAssistant.localPosition.y, 0.0f);
	}

	public void Reset(Transform clock, int restartInd)
	{
		teachAssistant.localPosition = new Vector3(0.0f, restartErrorOfYdirection[restartInd], 0.0f);
		teachAssistant.rotation = clock.rotation;
	}
}
