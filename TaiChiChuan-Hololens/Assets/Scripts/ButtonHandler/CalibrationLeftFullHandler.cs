using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class CalibrationLeftFullHandler : ButtonHandler
{
    private PressurePreProcessor pressurePreProcessor;

    private bool onRightFootLiftComplete = false;
    private GameObject waitHint;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        
        pressurePreProcessor = UdpNetworkServer.GetInstance().PressurePreProcessor;
        pressurePreProcessor.OnRightFootCompleteEvent += OnRightFootLiftComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if (onRightFootLiftComplete)
        {
            UnityEngine.Object.Destroy(waitHint);

            // Change color of button after calibration.
            WeightHintMoreControlPanel whmcp = transform.parent.GetComponent<WeightHintMoreControlPanel>();
            whmcp.RightFootLiftComplete();

            // Check if calibration steps are complete.
            if (whmcp.IsCalibrationStepComplete())
            {
                pressurePreProcessor.Apply();
                director.StartCapturingGlobalClicked();
                //director.DestroyControlPanel();
            }
        }
    }

    public override void OnInputClicked(InputClickedEventData eventData)
    {
        ProcessInputClicked(eventData);
    }

    protected override void ProcessInputClicked(InputClickedEventData eventData)
    {
        waitHint = WaitHintController.InstantiateGameObject();
        pressurePreProcessor.LeftFullCalibration();
    }

    public void OnRightFootLiftComplete()
    {
        onRightFootLiftComplete = true;
    }

    protected void OnDestroy()
    {
        pressurePreProcessor.OnRightFootCompleteEvent -= OnRightFootLiftComplete;
    }
}
