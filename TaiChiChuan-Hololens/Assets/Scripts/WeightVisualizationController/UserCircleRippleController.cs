using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCircleRippleController : CircleRippleController
{
    // For passing parameter when instantiate.
    class InitialParam
    {
        public Transform referenceCoach;
        public Transform referenceLeftFoot;
        public Transform referenceRightFoot;

        public InitialParam(Transform referenceCoach, Transform referenceLeftFoot, Transform referenceRightFoot)
        {
            this.referenceCoach = referenceCoach;
            this.referenceLeftFoot = referenceLeftFoot;
            this.referenceRightFoot = referenceRightFoot;
        }
    }
    static InitialParam initialParam;

    private Transform rotationCenter;
    private float rotateAngle;
    private float leftRatio = 0.5f;


    static public GameObject InstantiateGameObject(Transform parent, Transform referenceCoach, Transform referenceLeftFoot, Transform referenceRightFoot, Transform rotationCenter, float rotateAngle)
    {
        initialParam = new InitialParam(referenceCoach, referenceLeftFoot, referenceRightFoot);
        GameObject g = Instantiate(ResourcePool.GetInstance().GetCircleRipplePrefab(),
            referenceCoach.position, Quaternion.identity, parent);
        UserCircleRippleController controller = g.AddComponent<UserCircleRippleController>();

        controller.rotationCenter = rotationCenter;
        controller.rotateAngle = rotateAngle;
        UdpNetworkServer.GetInstance().PressurePreProcessor.OnPressureChangedEvent += controller.SetWeightRatio;

        initialParam = null;

        return g;
    }

    protected override void Awake()
    {
        referenceCoach = initialParam.referenceCoach;
        referenceLeftFoot = initialParam.referenceLeftFoot;
        referenceRightFoot = initialParam.referenceRightFoot;
    }

    protected override void Update()
    {
        base.Update();

        base.OnWeightDistributionChanged(leftRatio, 1.0f - leftRatio);
        this.transform.RotateAround(rotationCenter.position, Vector3.up, rotateAngle);
        this.transform.rotation *= Quaternion.Euler(-Vector3.up * rotateAngle);
        this.transform.position += Vector3.Normalize(this.transform.position - rotationCenter.position) * 0.3f;
    }

    private void SetWeightRatio(float leftRatio, float rightRatio)
    {
        this.leftRatio = leftRatio;
    }

    protected override void OnDestroy()
    {
        UdpNetworkServer.GetInstance().PressurePreProcessor.OnPressureChangedEvent -= SetWeightRatio;
    }
}
