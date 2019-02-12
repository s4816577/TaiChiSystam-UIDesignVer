using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFlatFootprintController : FlatFootprintController 
{
   	// For passing parameter when instantiate.
	class InitialParam
	{
		public Transform referenceCoach;
		public Transform referenceLeftFoot;
		public Transform referenceRightFoot;

		public InitialParam(Transform referenceCoach, Transform referenceLeftFoot, Transform referenceRightFoot)
		{
			this.referenceCoach = referenceCoach;
			this.referenceLeftFoot = referenceLeftFoot;
			this.referenceRightFoot = referenceRightFoot;
		}
	}
	static InitialParam initialParam;


	static public GameObject InstantiateGameObject(Transform parent, Transform referenceCoach, Transform referenceLeftFoot, Transform referenceRightFoot)
	{
		initialParam = new InitialParam(referenceCoach, referenceLeftFoot, referenceRightFoot);
		GameObject g = Instantiate(ResourcePool.GetInstance().GetFlatFootprintPrefab(), referenceCoach.position, Quaternion.identity, parent);
		UserFlatFootprintController controller = g.AddComponent<UserFlatFootprintController>();

		WeightDistributionPublisher weightDistributionPublisher = Object.FindObjectOfType<WeightDistributionPublisher>();
		weightDistributionPublisher.WeightDistributionChangedEvent += controller.OnWeightDistributionChanged;

		initialParam = null;

		return g;
	}

	protected override void Awake()
	{
		referenceCoach = initialParam.referenceCoach;
		referenceLeftFoot = initialParam.referenceLeftFoot;
		referenceRightFoot = initialParam.referenceRightFoot;
	}

	protected override void OnDestroy()
	{
		WeightDistributionPublisher weightDistributionPublisher = Object.FindObjectOfType<WeightDistributionPublisher>();
		weightDistributionPublisher.WeightDistributionChangedEvent -= base.OnWeightDistributionChanged;
	}

	protected override void Update()
	{
		Transform left = leftFootprintList[0];
		Transform right = rightFootprintList[0];

		this.transform.position = referenceCoach.position;
		// Left position.
		Vector3 currentPos = referenceLeftFoot.position + referenceLeftFoot.TransformDirection(leftShift + new Vector3(0.0014f, 0.01f, -0.015f));
		currentPos.y = referenceCoach.position.y;
		left.position = currentPos;
		// Right position.
		currentPos = referenceRightFoot.position + referenceRightFoot.TransformDirection(rightShift + new Vector3(0.0014f, -0.03f, -0.015f));
		currentPos.y = referenceCoach.position.y;
		right.position = currentPos;

		// Left rotation.
		Vector3 currRotation = referenceLeftFoot.rotation.eulerAngles;
		currRotation.x = currRotation.z = 0;
		left.rotation = Quaternion.Euler(currRotation) * Quaternion.Euler(0, -90, 0);
		// Right rotation.
		currRotation = referenceRightFoot.rotation.eulerAngles;
		currRotation.x = currRotation.z = 0;
		right.rotation = Quaternion.Euler(currRotation) * Quaternion.Euler(0, -90, 0);

		for (int i = 1; i < footprintList.Count; ++i)
		{
			leftFootprintList[i].position = left.position - Vector3.up * (i * FOOTPRINT_VERTICAL_GAP);
			leftFootprintList[i].rotation = left.rotation;
			rightFootprintList[i].position = right.position - Vector3.up * (i * FOOTPRINT_VERTICAL_GAP);
			rightFootprintList[i].rotation = right.rotation;
		}
	}
}
