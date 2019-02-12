using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachCircleRippleController : CircleRippleController
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


    static public GameObject InstantiateGameObject(Transform parent, Transform referenceCoach, Transform referenceLeftFoot, Transform referenceRightFoot)
    {
        initialParam = new InitialParam(referenceCoach, referenceLeftFoot, referenceRightFoot);
        GameObject g = Instantiate(ResourcePool.GetInstance().GetCircleRipplePrefab(), referenceCoach.position, Quaternion.identity, parent);
        CoachCircleRippleController controller = g.AddComponent<CoachCircleRippleController>();

        WeightDistributionPublisher weightDistributionPublisher = Object.FindObjectOfType<WeightDistributionPublisher>();
        weightDistributionPublisher.WeightDistributionChangedEvent += controller.OnWeightDistributionChanged;

        initialParam = null;

        return g;
    }

    protected override void Awake()
    {
        referenceCoach = initialParam.referenceCoach;
        referenceLeftFoot = initialParam.referenceLeftFoot;
        referenceRightFoot = initialParam.referenceRightFoot;
    }

    protected override void OnDestroy()
    {
        WeightDistributionPublisher weightDistributionPublisher = Object.FindObjectOfType<WeightDistributionPublisher>();
        weightDistributionPublisher.WeightDistributionChangedEvent -= base.OnWeightDistributionChanged;
    }
}
