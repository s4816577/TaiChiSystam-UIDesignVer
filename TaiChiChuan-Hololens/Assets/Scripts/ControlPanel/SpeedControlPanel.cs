using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedControlPanel : ControlPanel
{

	static public GameObject InstantiateGameObject()
	{
		GameObject g = Instantiate(ResourcePool.GetInstance().GetSpeedControlPanelPrefab());

		//g.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

		return g;
	}
}
