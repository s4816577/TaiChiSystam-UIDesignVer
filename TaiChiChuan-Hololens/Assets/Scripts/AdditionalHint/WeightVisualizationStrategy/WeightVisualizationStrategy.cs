using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class WeightVisualizationStrategy : IDisposable
{
    // Empty GameObject for WeightVisualization
    protected Transform weightVisualization;

    public WeightVisualizationStrategy()
    {
        GameObject g = new GameObject();
        g.name = "WeightVisualization";
        weightVisualization = g.transform;
    }

    public virtual void Dispose()
    {
        UnityEngine.Object.Destroy(weightVisualization.gameObject);
    }
}
