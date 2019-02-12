using System;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class ToggleAnimationModeHandler : ButtonHandler
{
    private ToggleHintController modeToggleHint;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        modeToggleHint = transform.parent.Find("ModeToggleHint").GetComponent<ToggleHintController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void ProcessInputClicked(InputClickedEventData eventData)
    {
        if (director.AnimationManager is ContinuousModeAnimationManager)
        {
            modeToggleHint.SetLeft();
            director.DetailedMode();
        }
        else if (director.AnimationManager is DetailedModeAnimationManager)
        {
            modeToggleHint.SetRight();
            director.CountinuousMode();
        }
    }
}
