using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ChangeGroundingInterfaceHandler : ButtonHandler
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        director = UnityEngine.Object.FindObjectOfType<Director>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void ProcessInputClicked(InputClickedEventData eventData)
    {
        director.SetGroundingInterfaceAdditionalHint();
    }
}
