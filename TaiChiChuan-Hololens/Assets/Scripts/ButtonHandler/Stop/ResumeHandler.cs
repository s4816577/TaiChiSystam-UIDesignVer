using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ResumeHandler : ButtonHandler
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
        director.SetNormalAdditionalHint();
        director.Play();
    }
}
