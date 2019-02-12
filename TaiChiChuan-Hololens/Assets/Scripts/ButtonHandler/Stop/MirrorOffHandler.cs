using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class MirrorOffHandler : ButtonHandler
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //mirrorToggleHint = transform.parent.Find("MirrorToggleHint").GetComponent<ToggleHintController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void ProcessInputClicked(InputClickedEventData eventData)
    {
        director.SetNormalAdditionalHint();
    }
}
