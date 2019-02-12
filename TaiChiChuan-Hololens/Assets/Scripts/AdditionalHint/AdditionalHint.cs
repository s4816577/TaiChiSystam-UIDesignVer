using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AdditionalHint : IDisposable
{
    protected AvatarsController avatarsController;

    public AdditionalHint(AvatarsController avatarsController)
    {
        this.avatarsController = avatarsController;
    }

    public virtual void Dispose()
    {
        //UnityEngine.Object.Destroy(weightVisualization.gameObject);
    }
}
