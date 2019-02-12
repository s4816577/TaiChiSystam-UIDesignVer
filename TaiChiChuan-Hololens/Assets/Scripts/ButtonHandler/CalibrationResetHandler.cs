using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class CalibrationResetHandler : ButtonHandler
{
    private PressurePreProcessor pressurePreProcessor;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        pressurePreProcessor = UdpNetworkServer.GetInstance().PressurePreProcessor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void ProcessInputClicked(InputClickedEventData eventData)
    {
        pressurePreProcessor.Reset();
    }
}
