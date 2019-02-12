using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class DescriptionHandler : ButtonHandler
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

		// Not necessary to StartCapturingGolbalClicked() here, 
		// it will be done on NavigationGesture end.
		// TODO show height animation here
	}

	protected override void ProcessInputClicked(InputClickedEventData eventData)
	{
		director.stageCode.Add(5);
		if (director.singleMode)
			director.singleMode = false;
		if (director.seriesMode)
			director.seriesMode = false;
		director.SetDescriptionControlPanel();
	}
}
