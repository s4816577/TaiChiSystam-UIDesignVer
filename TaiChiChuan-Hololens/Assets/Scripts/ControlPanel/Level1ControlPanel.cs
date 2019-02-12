using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1ControlPanel : ControlPanel
{
	static public GameObject InstantiateGameObject()
    {
        GameObject g = Instantiate(ResourcePool.GetInstance().GetLevel1ControlPanelPrefab());

		//g.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

		return g;
    }
}
