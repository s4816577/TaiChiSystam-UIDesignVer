﻿using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class Level1MoreHandler : ButtonHandler
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
        director.SetLevel2ControlPanel();
    }
}
