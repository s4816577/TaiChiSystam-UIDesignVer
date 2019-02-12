﻿using System;
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
		director.stageCode.Add(1);
		director.seriesMode = true;
		director.SetRestartInd(0);
		if (director.singleMode)
			director.singleMode = false;
	}
}
