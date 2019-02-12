using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class CalibrationRightFullHandler : ButtonHandler
{
    private PressurePreProcessor pressurePreProcessor;

    private bool onLeftFootLiftComplete = false;
    private GameObject waitHint;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        pressurePreProcessor = UdpNetworkServer.GetInstance().PressurePreProcessor;
        pressurePreProcessor.OnLeftFootCompleteEvent += OnLeftFootLiftComplete;
    }

    // Update is called once per frame
    void Update()
    {
        if (onLeftFootLiftComplete)
        {
            UnityEngine.Object.Destroy(waitHint);

            // Change color of button after calibration.
            WeightHintMoreControlPanel whmcp = transform.parent.GetComponent<WeightHintMoreControlPanel>();
            whmcp.LeftFootLiftComplete();

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
        pressurePreProcessor.RightFullCalibration();
    }

    public void OnLeftFootLiftComplete()
    {
        onLeftFootLiftComplete = true;
    }

    protected void OnDestroy()
    {
        pressurePreProcessor.OnLeftFootCompleteEvent -= OnLeftFootLiftComplete;
    }
}
