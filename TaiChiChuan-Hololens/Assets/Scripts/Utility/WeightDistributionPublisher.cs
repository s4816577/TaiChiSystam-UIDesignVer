using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightDistributionPublisher : MonoBehaviour
{
    public delegate void WeightDistributionChangedHandler(float leftRatio, float rightRatio);
    public event WeightDistributionChangedHandler WeightDistributionChangedEvent;

    private Transform referenceLeftFoot, referenceRightFoot;

    private float leftRatio = 0.0f;
    public float LeftRatio { get { return leftRatio; } }

    // Use this for initialization
    private void Start()
    {
        referenceLeftFoot = transform.Find("Taiji_CC_Base_Root/Taiji_CC_Base_Hip/Taiji_CC_Base_Pelvis/Taiji_CC_Base_L_Thigh/Taiji_CC_Base_L_Calf/Taiji_CC_Base_L_Foot");
        referenceRightFoot = transform.Find("Taiji_CC_Base_Root/Taiji_CC_Base_Hip/Taiji_CC_Base_Pelvis/Taiji_CC_Base_R_Thigh/Taiji_CC_Base_R_Calf/Taiji_CC_Base_R_Foot");

        Renderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (Renderer renderer in renderers)
            renderer.enabled = false;
        renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (Renderer renderer in renderers)
            renderer.material.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        const float THRESHOLD_OF_HEELING = 0.10f;

        Vector3 modelCenterOfMass = this.GetComponent<Rigidbody>().worldCenterOfMass;

        Vector3 leftFootPosition = referenceLeftFoot.position; // point
        Vector3 rightFootPosition = referenceRightFoot.position; // point

        Vector3 right2LeftVector = leftFootPosition - rightFootPosition; // vertor
        Vector3 right2ComVector = modelCenterOfMass - rightFootPosition; // vector

        float weightRateOfRightFoot = 1.0f - Vector3.Dot(right2LeftVector, right2ComVector) / right2LeftVector.magnitude / right2LeftVector.magnitude;

        //Debug.Log("left height = " + leftFootPosition.y + " right height = " + rightFootPosition.y + " differ = " + (leftFootPosition.y - rightFootPosition.y));
        //Debug.Log("rateOfR = " + weightRateOfRightFoot);

        //clip
        if (weightRateOfRightFoot > 1.0f)
        {
            weightRateOfRightFoot = 1.0f;
        }
        else if (weightRateOfRightFoot < 0.0f)
        {
            weightRateOfRightFoot = 0.0f;
        }
        else if ((rightFootPosition.y - leftFootPosition.y) > 0.0f) // 右腳抬起來
        {
            float gap = Mathf.Min(THRESHOLD_OF_HEELING, rightFootPosition.y - leftFootPosition.y);

            weightRateOfRightFoot = Mathf.Lerp(weightRateOfRightFoot, 0.0f, gap / THRESHOLD_OF_HEELING);
        }
        else if ((leftFootPosition.y - rightFootPosition.y) > 0.0f) //左腳抬起來
        {
            float gap = Mathf.Min(THRESHOLD_OF_HEELING, leftFootPosition.y - rightFootPosition.y);

            weightRateOfRightFoot = Mathf.Lerp(weightRateOfRightFoot, 1.0f, gap / THRESHOLD_OF_HEELING);
        }


        //Debug.Log("rateOfR = " + weightRateOfRightFoot);
        if (WeightDistributionChangedEvent != null)
        {
            WeightDistributionChangedEvent.Invoke(1.0f - weightRateOfRightFoot, weightRateOfRightFoot);
        }
        leftRatio = 1.0f - weightRateOfRightFoot;
    }
}
