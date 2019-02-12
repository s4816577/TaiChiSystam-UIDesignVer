using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightTransferRecorder : MonoBehaviour
{
    private float leftRatio = 0.5f;
    private float elapsedTime = 0.0f;

    private WeightDistributionPublisher weightDistributionPublisher;


    private void Start()
    {
        weightDistributionPublisher = Object.FindObjectOfType<WeightDistributionPublisher>();
        UdpNetworkServer.GetInstance().PressurePreProcessor.OnPressureChangedEvent += SetWeightRatio;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1.0f)
        {
            Logger.GetInstance().WriteLine(weightDistributionPublisher.LeftRatio.ToString() + "," + leftRatio);
            elapsedTime = 0.0f;
        }
    }

    private void SetWeightRatio(float leftRatio, float rightRatio)
    {
        this.leftRatio = leftRatio;
    }

    private void OnDestroy()
    {
        UdpNetworkServer.GetInstance().PressurePreProcessor.OnPressureChangedEvent -= SetWeightRatio;
    }
}
