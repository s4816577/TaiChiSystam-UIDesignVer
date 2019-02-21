using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class HintHandler : MonoBehaviour, HoloToolkit.Unity.InputModule.IInputClickHandler
{
	protected Director director;

	// Use this for initialization
	protected virtual void Start()
	{
		director = UnityEngine.Object.FindObjectOfType<Director>();
	}

	public virtual void OnInputClicked(InputClickedEventData eventData)
	{
		//director.Help();
	}

	void Update()
	{/*
		if (director.stageCode[director.stageCode.Count - 1] == 1 || (director.stageCode[director.stageCode.Count - 1] == 2 && !director.IsUsingControlPanel) || director.stageCode[director.stageCode.Count - 1] == 3)
		{
			MeshRenderer[] renderers = this.transform.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer renderer in renderers)
				renderer.enabled = true;
		}
		else
		{
			MeshRenderer[] renderers = this.transform.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer renderer in renderers)
				renderer.enabled = false;
		}*/
	}
}
