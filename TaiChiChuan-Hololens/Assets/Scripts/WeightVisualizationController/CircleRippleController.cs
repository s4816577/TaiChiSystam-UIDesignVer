using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class CircleRippleController : MonoBehaviour
{
    private const float BASIC_SCALE = 1.0f;
    private const float ADDITIONAL_SCALE = 0.3f;

    private GameObject circleRipplePrefab;
    protected Transform referenceCoach;
    protected Transform referenceLeftFoot;
    protected Transform referenceRightFoot;

    private List<Transform> circleRippleList = new List<Transform>();
    private List<Transform> leftCircleRippleList = new List<Transform>();
    private List<Transform> rightCircleRippleList = new List<Transform>();
    Transform firstLeftFootprint;
    Transform firstRightFootprint;

    Vector3 leftShift, rightShift;


    protected abstract void Awake();

    // Use this for initialization
    protected virtual void Start()
    {
        circleRipplePrefab = transform.Find("CircleRipple").gameObject;
        circleRippleList.Add(circleRipplePrefab.transform);

        Transform firstLeftCircleRipple = circleRipplePrefab.transform.Find("LeftCircleRipple");
        leftCircleRippleList.Add(firstLeftCircleRipple);
        Transform firstRightCircleRipple = circleRipplePrefab.transform.Find("RightCircleRipple");
        rightCircleRippleList.Add(firstRightCircleRipple);

        for (int i = 1; i < 10; i += 1)
        {
            circleRippleList.Add(Instantiate(circleRippleList[0].gameObject, this.transform).transform);
            leftCircleRippleList.Add(circleRippleList[i].Find("LeftCircleRipple"));
            rightCircleRippleList.Add(circleRippleList[i].Find("RightCircleRipple"));

            MeshRenderer[] originRenderer = circleRipplePrefab.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer[] newRenderer = circleRippleList[i].GetComponentsInChildren<MeshRenderer>();

            // Create transparent effect.
            for (int j = 0; j < originRenderer.Length; ++j)
            {
                Material newMaterial = new Material(originRenderer[j].material);
                newMaterial.color = new Color(newMaterial.color.r, newMaterial.color.g, newMaterial.color.b, newMaterial.color.a - (80.0f + i * 20) / 255.0f);
                newRenderer[j].material = newMaterial;
            }
        }

        firstLeftFootprint = transform.Find("Footprint/LeftFootprint");
        firstRightFootprint = transform.Find("Footprint/RightFootprint");

        // Find shift vector in local space.
        CoachAvatar tmpCoach = new CoachAvatar(Instantiate(ResourcePool.GetInstance().GetCoachModelPrefab(), transform.position, Quaternion.identity));

        leftShift = firstLeftFootprint.position - tmpCoach.LeftFoot.position;
        leftShift = tmpCoach.LeftFoot.InverseTransformDirection(leftShift);
        rightShift = firstRightFootprint.position - tmpCoach.RightFoot.position;
        rightShift = tmpCoach.RightFoot.InverseTransformDirection(rightShift);

        UnityEngine.Object.Destroy(tmpCoach.Avatar.parent.gameObject);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Transform left = leftCircleRippleList[0];
        Transform right = rightCircleRippleList[0];

        this.transform.position = referenceCoach.position;
        // Left position.
        Vector3 currentPos = referenceLeftFoot.position + referenceLeftFoot.TransformDirection(leftShift);
        currentPos.y = referenceCoach.position.y;
        left.position = currentPos;
        // Right position.
        currentPos = referenceRightFoot.position + referenceRightFoot.TransformDirection(rightShift);
        currentPos.y = referenceCoach.position.y;
        right.position = currentPos;

        // Left rotation.
        Vector3 currRotation = referenceLeftFoot.rotation.eulerAngles;
        currRotation.x = currRotation.z = 0;
        left.rotation = Quaternion.Euler(currRotation);
        // Right rotation.
        currRotation = referenceRightFoot.rotation.eulerAngles;
        currRotation.x = currRotation.z = 0;
        right.rotation = Quaternion.Euler(currRotation);

        for (int i = 1; i < circleRippleList.Count; ++i)
        {
            leftCircleRippleList[i].position = left.position;
            rightCircleRippleList[i].position = right.position;
            leftCircleRippleList[i].rotation = left.rotation;
            rightCircleRippleList[i].rotation = right.rotation;
            leftCircleRippleList[i].localScale = Vector3.one * (BASIC_SCALE + i * ADDITIONAL_SCALE);
            rightCircleRippleList[i].localScale = Vector3.one * (BASIC_SCALE + i * ADDITIONAL_SCALE);
        }
        
        if (firstLeftFootprint != null)
        {
            firstLeftFootprint.position = leftCircleRippleList[0].position;
            firstLeftFootprint.rotation = leftCircleRippleList[0].rotation;
        }
        if (firstRightFootprint != null)
        {
            firstRightFootprint.position = rightCircleRippleList[0].position;
            firstRightFootprint.rotation = rightCircleRippleList[0].rotation;
        }
    }

    protected void OnWeightDistributionChanged(float leftRatio, float rightRatio)
    {
        int leftLevel = (int)Mathf.Round(leftRatio * 10 + 0.5f);
        int rightLevel = 10 - leftLevel;

        for (int i = 0; i < leftCircleRippleList.Count; ++i)
        {
            leftCircleRippleList[i].gameObject.SetActive(i < leftLevel);
            rightCircleRippleList[i].gameObject.SetActive(i < rightLevel);
        }
    }

    protected abstract void OnDestroy();
}
