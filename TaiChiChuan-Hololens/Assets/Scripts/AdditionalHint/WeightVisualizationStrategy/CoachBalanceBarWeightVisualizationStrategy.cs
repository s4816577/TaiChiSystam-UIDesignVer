using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoachBalanceBarWeightVisualizationStrategy : WeightVisualizationStrategy
{
    private GameObject g;

    public CoachBalanceBarWeightVisualizationStrategy()
        : base()
    {
        g = CoachBalanceBarController.InstantiateGameObject();
    }

    public override void Dispose()
    {
        base.Dispose();
        UnityEngine.Object.Destroy(g);
    }
}
