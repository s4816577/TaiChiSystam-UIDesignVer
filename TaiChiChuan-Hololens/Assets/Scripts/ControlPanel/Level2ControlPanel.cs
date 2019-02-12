using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2ControlPanel : ControlPanel
{
    private ToggleHintController modeToggleHint;
    private ToggleHintController mirrorToggleHint;
    private ToggleHintController weightToggleHint;

    static public GameObject InstantiateGameObject()
    {
        GameObject g = Instantiate(ResourcePool.GetInstance().GetLevel2ControlPanelPrefab());

        //g.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

        return g;
    }

    protected override void Start()
    {
        base.Start();

        //Director director = UnityEngine.Object.FindObjectOfType<Director>();

        //modeToggleHint = transform.Find("ModeToggleHint").GetComponent<ToggleHintController>();
        //mirrorToggleHint = transform.Find("MirrorToggleHint").GetComponent<ToggleHintController>();
        //weightToggleHint = transform.Find("WeightToggleHint").GetComponent<ToggleHintController>();

        //if (director.AnimationManager is DetailedModeAnimationManager)
        //{
            //modeToggleHint.SetRight();
        //}

        //if (director.AdditionalHint is MirrorAdditionalHint)
        //{
        //    mirrorToggleHint.SetLeft();
        //}

        // If AdditionalHint is one of Weight Distribution Hint, then set toggl hint to right side,
        // including "Grounding", "Ripple" and "Scrollbar" hint.
        //if (director.AdditionalHint is TeachAssistantAdditionalHint &&
        //    !(director.AdditionalHint is NormalTeachAssistantAdditionalHint))
        //{
        //    weightToggleHint.SetRight();
        //}
    }
}
