using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserBalanceBarController : BalanceBarController 
{
    private float leftRatio = 0.5f;


    static public GameObject InstantiateGameObject()
    {
        GameObject g = Instantiate(ResourcePool.GetInstance().GetBalanceBarPrefab(), UserInterface.GetInstance().transform);
        UserBalanceBarController controller = g.AddComponent<UserBalanceBarController>();

        UdpNetworkServer.GetInstance().PressurePreProcessor.OnPressureChangedEvent += controller.SetWeightRatio;

        return g;
    }

    protected override void Awake()
    {
    }

    protected override void Start()
    {
        base.Start();

        target = transform.Find("Taichi");
        target.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();

        base.OnWeightDistributionChanged(leftRatio, 1.0f - leftRatio);
    }

    private void SetWeightRatio(float leftRatio, float rightRatio)
    {
        this.leftRatio = leftRatio;
    }

    protected override void OnDestroy()
    {
        UdpNetworkServer.GetInstance().PressurePreProcessor.OnPressureChangedEvent -= SetWeightRatio;
    }
}
