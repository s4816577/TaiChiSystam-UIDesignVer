﻿using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ToggleWeightHintHandler : ButtonHandler
{
    private ToggleHintController weightToggleHint;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //weightToggleHint = transform.parent.Find("WeightToggleHint").GetComponent<ToggleHintController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void ProcessInputClicked(InputClickedEventData eventData)
    {
        if (director.AdditionalHint is TeachAssistantAdditionalHint &&
            !(director.AdditionalHint is NormalTeachAssistantAdditionalHint))
        {
            //weightToggleHint.SetLeft();
            director.SetNormalAdditionalHint();
        }
        else
        {
            //weightToggleHint.SetRight();
            director.SetGroundingInterfaceAdditionalHint();
        }
    }
}
