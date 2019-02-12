using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionControlPanel : ControlPanel
{

	static public GameObject InstantiateGameObject()
	{
		GameObject g = Instantiate(ResourcePool.GetInstance().GetDescriptionControlPanelPrefab());

		//g.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

		return g;
	}
}
