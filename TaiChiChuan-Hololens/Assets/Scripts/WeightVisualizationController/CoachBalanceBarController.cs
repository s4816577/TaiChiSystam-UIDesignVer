using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachBalanceBarController : BalanceBarController 
{
    static public GameObject InstantiateGameObject()
    {
        GameObject g = Instantiate(ResourcePool.GetInstance().GetBalanceBarPrefab(), UserInterface.GetInstance().transform);
        CoachBalanceBarController controller = g.AddComponent<CoachBalanceBarController>();

        WeightDistributionPublisher weightDistributionPublisher = Object.FindObjectOfType<WeightDistributionPublisher>();
        weightDistributionPublisher.WeightDistributionChangedEvent += controller.OnWeightDistributionChanged;

        return g;
    }

    protected override void Awake()
    {
    }

    protected override void Start()
    {
        base.Start();

        target = transform.Find("Seafood");
        target.gameObject.SetActive(true);
    }

    protected override void OnDestroy()
    {
        WeightDistributionPublisher weightDistributionPublisher = Object.FindObjectOfType<WeightDistributionPublisher>();
        weightDistributionPublisher.WeightDistributionChangedEvent -= base.OnWeightDistributionChanged;
    }
}
