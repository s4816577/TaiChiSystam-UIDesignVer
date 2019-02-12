using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUnderFootCircleRippleController : CircleRippleController
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

    private float leftRatio = 0.5f;
    private CoachAvatar referenceAvatar;

    static public GameObject InstantiateGameObject(Transform parent, CoachAvatar referenceAvatar, Transform referenceLeftFoot, Transform referenceRightFoot)
    {
        Transform referenceCoach = referenceAvatar.Avatar.parent;
        initialParam = new InitialParam(referenceCoach, referenceLeftFoot, referenceRightFoot);
        GameObject g = Instantiate(ResourcePool.GetInstance().GetCircleRipplePrefab(),
            referenceCoach.position, Quaternion.identity, parent);
        UserUnderFootCircleRippleController controller = g.AddComponent<UserUnderFootCircleRippleController>();

        g.transform.parent.parent = referenceCoach;
        referenceCoach.transform.localScale *= 3.0f;
        g.transform.parent.parent = null;

        controller.referenceAvatar = referenceAvatar;
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
        this.transform.position = referenceCoach.position - Vector3.up * 1.5f;
        this.transform.Translate(-0.3f * Camera.main.transform.up);
    }

    private void SetWeightRatio(float leftRatio, float rightRatio)
    {
        this.leftRatio = leftRatio;
    }

    protected override void OnDestroy()
    {
        referenceCoach.transform.localScale /= 3.0f;
        UdpNetworkServer.GetInstance().PressurePreProcessor.OnPressureChangedEvent -= SetWeightRatio;
    }
}
