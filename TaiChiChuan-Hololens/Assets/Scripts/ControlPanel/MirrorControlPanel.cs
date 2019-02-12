using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorControlPanel : ControlPanel
{
    static public GameObject InstantiateGameObject()
    {
        GameObject g = Instantiate(ResourcePool.GetInstance().GetMirrorControlPanelPrefab());

        //g.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

        return g;
    }
}