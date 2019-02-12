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
        teachAssistant.localPosition = Vector3.zero;
    }

	public void Reset(Transform clock, int restartInd)
	{
		teachAssistant.localPosition = Vector3.zero;
		teachAssistant.rotation = clock.rotation;
	}
}
