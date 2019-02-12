using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BalanceBarController : MonoBehaviour
{
    private Transform leftFoot;
    private Transform rightFoot;
    protected Transform target;

    protected abstract void Awake();

    // Use this for initialization
    protected virtual void Start()
    {
        leftFoot = this.transform.Find("LeftFoot");
        rightFoot = this.transform.Find("RightFoot");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    protected void OnWeightDistributionChanged(float leftRatio, float rightRatio)
    {
        int leftLevel = (int)Mathf.Round(leftRatio * 10 + 0.5f);
        int rightLevel = 10 - leftLevel;

        target.position = (float)leftLevel / 10.0f * leftFoot.position + (float)rightLevel / 10.0f * rightFoot.position;
    }

    protected abstract void OnDestroy();
}
