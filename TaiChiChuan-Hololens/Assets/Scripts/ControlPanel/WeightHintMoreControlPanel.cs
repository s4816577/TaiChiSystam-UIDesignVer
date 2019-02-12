using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightHintMoreControlPanel : ControlPanel
{
    private Transform rightFootLift;
    private Transform rightFootLiftComplete;
    private Transform leftFootLift;
    private Transform leftFootLiftComplete;

    static public GameObject InstantiateGameObject()
    {
        GameObject g = Instantiate(ResourcePool.GetInstance().GetWeightHintMoreControlPanelPrefab());

        return g;
    }

    protected override void Start()
    {

    }

    protected override void Awake()
    {
        base.Awake();

        rightFootLift = transform.Find("RightFootLift");
        rightFootLiftComplete = transform.Find("RightFootLiftComplete");
        leftFootLift = transform.Find("LeftFootLift");
        leftFootLiftComplete = transform.Find("LeftFootLiftComplete");
    }

    public void RightFootLiftComplete()
    {
        rightFootLift.gameObject.SetActive(false);
        rightFootLiftComplete.gameObject.SetActive(true);
    }

    public void LeftFootLiftComplete()
    {
        leftFootLift.gameObject.SetActive(false);
        leftFootLiftComplete.gameObject.SetActive(true);
    }

    public bool IsCalibrationStepComplete()
    {
        return leftFootLiftComplete.gameObject.activeInHierarchy && rightFootLiftComplete.gameObject.activeInHierarchy;
    }
}
