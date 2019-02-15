using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class NormalSpeedHandler : ButtonHandler
{
	// Use this for initialization
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public override void OnInputClicked(InputClickedEventData eventData)
	{
		ProcessInputClicked(eventData);
	}

	protected override void ProcessInputClicked(InputClickedEventData eventData)
	{
		director.SetInitSpeed(1.0f);
		RepeatPlaying[] script = GameObject.FindObjectsOfType<RepeatPlaying>();
		script[0].SetPlayingSpeed(1.0f);
	}
}
