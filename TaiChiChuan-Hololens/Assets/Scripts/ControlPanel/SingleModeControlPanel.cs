using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleModeControlPanel : ControlPanel
{
	static public GameObject InstantiateGameObject()
	{
		GameObject g = Instantiate(ResourcePool.GetInstance().GetSingleModeControlPanelPrefab());

		//g.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

		return g;
	}
}
