using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class SeriesHandler : ButtonHandler
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

	protected override void ProcessInputClicked(InputClickedEventData eventData)
	{
		director.SaveInformation("進入套路模式");
		director.NotShowingPauseLog();
		director.stageCode.Add(1);
		director.seriesMode = true;
		director.ResetCount();
		director.PlaySoundOfSeriesMode();
		director.SetRestartInd(0);
		if (director.singleMode)
			director.singleMode = false;
	}
}
