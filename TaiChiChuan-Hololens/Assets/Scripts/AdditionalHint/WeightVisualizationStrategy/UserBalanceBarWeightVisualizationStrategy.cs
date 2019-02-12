using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserBalanceBarWeightVisualizationStrategy : WeightVisualizationStrategy
{
    private GameObject g;

    public UserBalanceBarWeightVisualizationStrategy()
        : base()
    {
        g = UserBalanceBarController.InstantiateGameObject();
    }

    public override void Dispose()
    {
        base.Dispose();
        UnityEngine.Object.Destroy(g);
    }
}
