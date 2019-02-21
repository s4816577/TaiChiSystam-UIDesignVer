using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpControlPanel : ControlPanel
{

	static public GameObject InstantiateGameObject(int stage)
	{
		GameObject g = Instantiate(ResourcePool.GetInstance().GetHelpControlPanelPrefab(stage));

		//g.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

		return g;
	}
}
