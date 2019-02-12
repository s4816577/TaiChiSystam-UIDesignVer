using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class FlatFootprintController : MonoBehaviour
{
	protected const float FOOTPRINT_VERTICAL_GAP = 0.01f;
	private const float FootprintScale = 0.8f; 

	protected GameObject footprintPrefab;
    protected Transform referenceCoach;
    protected Transform referenceLeftFoot;
    protected Transform referenceRightFoot;

	/// <summary>
	/// All footprint objects under coach. (with 10 level for each foot)
	/// Dynamically create all footprint objects at run-time by footprintPrefab.
	/// </summary>
	protected List<Transform> footprintList = new List<Transform>();
	protected List<Transform> leftFootprintList = new List<Transform>();
	protected List<Transform> rightFootprintList = new List<Transform>();

	protected Vector3 leftShift, rightShift;


    protected abstract void Awake();

    // Use this for initialization
    protected virtual void Start()
    {
        footprintPrefab = transform.Find("Footprint").gameObject;
		footprintPrefab.transform.Find("LeftFootprint").transform.localScale = new Vector3(FootprintScale, FootprintScale, FootprintScale);
		footprintPrefab.transform.Find("RightFootprint").transform.localScale = new Vector3(FootprintScale, FootprintScale, FootprintScale);
		footprintList.Add(footprintPrefab.transform);

        Transform firstLeftFootprint = footprintPrefab.transform.Find("LeftFootprint");
        leftFootprintList.Add(firstLeftFootprint);
        Transform firstRightFootprint = footprintPrefab.transform.Find("RightFootprint");
        rightFootprintList.Add(firstRightFootprint);

        for (int i = 1; i < 10; i += 1)
        {
            footprintList.Add(Instantiate(footprintList[0].gameObject, this.transform).transform);
            leftFootprintList.Add(footprintList[i].Find("LeftFootprint"));
            rightFootprintList.Add(footprintList[i].Find("RightFootprint"));

            MeshRenderer[] originRenderer = footprintPrefab.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer[] newRenderer = footprintList[i].GetComponentsInChildren<MeshRenderer>();

            // Create transparent effect.
            for (int j = 0; j < originRenderer.Length; ++j)
            {
                Material newMaterial = new Material(originRenderer[j].material);
                newMaterial.color = new Color(newMaterial.color.r, newMaterial.color.g, newMaterial.color.b, newMaterial.color.a - (80.0f + i * 20) / 255.0f);
                newRenderer[j].material = newMaterial;
            }
        }

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
        Transform left = leftFootprintList[0];
        Transform right = rightFootprintList[0];

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

        for (int i = 1; i < footprintList.Count; ++i)
        {
            leftFootprintList[i].position = left.position - Vector3.up * (i * FOOTPRINT_VERTICAL_GAP);
            leftFootprintList[i].rotation = left.rotation;
            rightFootprintList[i].position = right.position - Vector3.up * (i * FOOTPRINT_VERTICAL_GAP);
            rightFootprintList[i].rotation = right.rotation;
        }
    }

    protected void OnWeightDistributionChanged(float leftRatio, float rightRatio)
    {
        int leftLevel = (int)Mathf.Round(leftRatio * 10 + 0.5f);
        int rightLevel = 10 - leftLevel;

        for (int i = 0; i < leftFootprintList.Count; ++i)
        {
            leftFootprintList[i].gameObject.SetActive(i < leftLevel);
            rightFootprintList[i].gameObject.SetActive(i < rightLevel);
        }
    }

    protected abstract void OnDestroy();
}
