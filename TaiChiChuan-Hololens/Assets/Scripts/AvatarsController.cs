using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarsController : MonoBehaviour
{
    static private AvatarsController instance = null;

    private const int NUM_OF_COACHES = 8;
    private const float DISTANCE2COACHES = 1.3f;
    private const float DISTANCE2TAS = 1.6f;
	private const float DISTANCE2CLOCKS = 2f;
	private const float AvatersScale = 0.8f;

    public float AvatarsHeight = -0.34f;
	public float AvatarsBaseHeight = -0.34f;
	public float ClockHeight = 0f;

	// For Coaches;
	private Transform coachGroup;
    private List<CoachAvatar> coaches = new List<CoachAvatar>();
    public List<CoachAvatar> Coaches { get { return coaches; } }

    // For Reference Coaches
    private CoachAvatar referenceCoach;
    public CoachAvatar ReferenceCoach { get { return referenceCoach; } }

    // For Teach Assistants
    private Transform teachAssistantGroup;
    private List<TeachAssistantAvatar> teachAssistants = new List<TeachAssistantAvatar>();
    public List<TeachAssistantAvatar> TeachAssistants { get { return teachAssistants; } }

	//For clock 
	private Transform clockGroup;

    private ICoachPositionMode coachPositionMode = new FollowedCoachPositionMode();

    static public AvatarsController GetInstance()
    {
        if (instance == null)
        {
            GameObject g = Instantiate(ResourcePool.GetInstance().GetAvatarsPrefab());
            instance = g.GetComponent<AvatarsController>();
        }

        return instance;
    }

    private void Awake()
    {
        coachGroup = this.transform.Find("CoachGroup");
        teachAssistantGroup = this.transform.Find("TeachAssistantGroup");
		clockGroup = this.transform.Find("ClockGroup");

		// Generate all coaches around camera.
		GameObject coachModelPrefab = ResourcePool.GetInstance().GetCoachModelPrefab();
        for (int i = 0; i < NUM_OF_COACHES; ++i)
        {
            Vector3 direction = Quaternion.AngleAxis(360.0f / NUM_OF_COACHES * i, Vector3.up) * Vector3.forward;
            GameObject g = Instantiate(coachModelPrefab,
                transform.position + DISTANCE2COACHES * direction, Quaternion.identity, coachGroup) as GameObject;
			g.transform.localScale = new Vector3(AvatersScale, AvatersScale, AvatersScale);
            coaches.Add(new CoachAvatar(g));
        }

        // Generate all TAs around camera.
        for (int i = 0; i < NUM_OF_COACHES; ++i)
        {
            Vector3 direction = Quaternion.AngleAxis(360.0f / NUM_OF_COACHES / 2 + 360.0f / NUM_OF_COACHES * i, Vector3.up) * Vector3.forward;
            GameObject gameObj = new GameObject
            {
                name = "TaichiTAObject"
            };
            gameObj.transform.parent = teachAssistantGroup;
            gameObj.transform.position = DISTANCE2TAS * direction;

            GameObject taModelPrefab = ResourcePool.GetInstance().GetTeachAssistantModelPrefab();
            GameObject g = Instantiate(taModelPrefab,
                Vector3.zero, Quaternion.identity, gameObj.transform) as GameObject;
            g.name = TeachAssistantAvatar.TA_AVATAR_NAME;
			gameObj.transform.localScale = new Vector3(AvatersScale, AvatersScale, AvatersScale);
			GameObject carpet = Instantiate(ResourcePool.GetInstance().GetCarpetPrefab(), gameObj.transform);

            teachAssistants.Add(new TeachAssistantAvatar(gameObj));
        }

        // Instantiate reference coach;
        GameObject referenceCoachObject = Instantiate(ResourcePool.GetInstance().GetCoachModelPrefab()) as GameObject;
        referenceCoachObject.name = "Reference Coach";
        referenceCoach = new CoachAvatar(referenceCoachObject);
        referenceCoach.Avatar.GetComponent<Animator>().cullingMode = AnimatorCullingMode.AlwaysAnimate;
        referenceCoach.Avatar.gameObject.AddComponent<Rigidbody>();
        referenceCoach.Avatar.gameObject.gameObject.AddComponent<WeightDistributionPublisher>();
		
		// TODO
		// Update avatarsHeight once to avoid wrong position after Awake();
		//coachPositionMode.UpdateCoachPosition(Camera.main.transform, AvatarsHeight, this.transform);
	}

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        coachPositionMode.UpdateCoachPosition(Camera.main.transform, AvatarsHeight, coachGroup);
		coachPositionMode.UpdateCoachPosition(Camera.main.transform, AvatarsHeight, teachAssistantGroup);
		coachPositionMode.UpdateClockPosition(Camera.main.transform, (AvatarsHeight - AvatarsBaseHeight) * DISTANCE2CLOCKS / DISTANCE2COACHES + ClockHeight, clockGroup);

		foreach (CoachAvatar coachAvatar in coaches)
        {
            coachAvatar.Update();
        }
        foreach (TeachAssistantAvatar taAvatar in teachAssistants)
        {
            taAvatar.Update();
        }

        referenceCoach.Avatar.parent.position = Camera.main.transform.position;
        referenceCoach.Update();
    }

    public void SetCoachPositionMode(ICoachPositionMode coachPositionMode)
    {
        this.coachPositionMode = coachPositionMode;
    }

    private void OnDestroy()
    {
        Debug.Log("avatar destroy");
    }

	public void ResetAvatersPosition(int restartInd)
	{
		foreach (CoachAvatar coachAvatar in coaches)
		{
			coachAvatar.Reset(clockGroup, restartInd);
		}
		foreach (TeachAssistantAvatar taAvatar in teachAssistants)
		{
			taAvatar.Reset(clockGroup, restartInd);
		}
		referenceCoach.Reset(clockGroup, restartInd);
	}

	public void ActiveAvatars(bool IsActive)
	{
		foreach (IAvatar avatar in TeachAssistants)
		{
			Renderer[] renderers = avatar.Avatar.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (Renderer renderer in renderers)
				renderer.enabled = IsActive;

			renderers = avatar.Avatar.parent.GetComponentsInChildren<MeshRenderer>();
			foreach (Renderer renderer in renderers)
				renderer.enabled = IsActive;
		}

		foreach (IAvatar avatar in Coaches)
		{
			Renderer[] renderers = avatar.Avatar.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (Renderer renderer in renderers)
				renderer.enabled = IsActive;

			renderers = avatar.Avatar.parent.GetComponentsInChildren<MeshRenderer>();
			foreach (Renderer renderer in renderers)
				renderer.enabled = IsActive;
		}

		MeshRenderer[] renderers2 = clockGroup.transform.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers2)
			renderer.enabled = IsActive;
	}

	public void SetAvatarsHeight(float height)
	{
		AvatarsHeight = height;
	}

	public void SetToFront()
	{
		coachPositionMode.SetRotation();
	}
}
