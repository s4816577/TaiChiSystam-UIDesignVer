using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class UserInterface : MonoBehaviour
{
    static private UserInterface instance = null;

    private System.Threading.Thread mainThread;

    // Left UI
    private Transform leftUI;
    private TextMesh movementTextMesh;
    private MeshRenderer iconImageMeshRenderer;
    private TextMesh speedValueTextMesh;

    // Right UI
    private Transform rightUI;
    private TextMesh actionTextMesh;

    // Material resources.
    private Material pauseIconMaterial;
    private Material playIconMaterial;

    // Command Queue
    private string command = "";

    // For debug.
    private TextMesh debugTextMesh;
    private string bufferstr;

	//DISTANCE
	public const float DIS2UI = 1.8f;


    static public UserInterface GetInstance()
    {
        if (instance == null)
        {
            GameObject g = Instantiate(ResourcePool.GetInstance().GetUserInterfacePrefab());
			g.transform.position = Camera.main.transform.position + DIS2UI * Camera.main.transform.forward;
			g.transform.forward = Camera.main.transform.forward;
			instance = g.GetComponent<UserInterface>();
        }

        return instance;
    }

    private void Awake()
    {
        // Left UI
        leftUI = transform.Find("LeftUI");
        movementTextMesh = transform.Find("LeftUI/MovementName").GetComponent<TextMesh>();
        iconImageMeshRenderer = transform.Find("LeftUI/IconImage").GetComponent<MeshRenderer>();
        speedValueTextMesh = transform.Find("LeftUI/SpeedValue").GetComponent<TextMesh>();

        // Right UI
        rightUI = transform.Find("RightUI");
        actionTextMesh = transform.Find("RightUI/ActionName").GetComponent<TextMesh>();

        // Load material resources.
        pauseIconMaterial = Resources.Load("Materials/UIMaterials/pause_icon_material") as Material;
        playIconMaterial = Resources.Load("Materials/UIMaterials/play_icon_material") as Material;
        if (pauseIconMaterial == null || playIconMaterial == null)
            Debug.Log("Load Resouces Error!");

        // For debug.
        debugTextMesh = transform.Find("Debug").GetComponent<TextMesh>();
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (command != "")
        {
            CreateCommandName(command);
            command = "";
        }
		transform.position = Camera.main.transform.position + DIS2UI * Camera.main.transform.forward;
		transform.forward = Camera.main.transform.forward;
	}

    public void ShowRightUI(bool isShowing)
    {
        MeshRenderer[] renderers = rightUI.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in renderers)
            renderer.enabled = isShowing;
    }

    public void SetAnimationManagerMode(AnimationManager animationManager)
    {
        animationManager.AnimationDelegator.MovementChangeEvent += SetMovementName;
        animationManager.AnimationDelegator.ActionChangeEvent += SetActionName;
        animationManager.AnimationDelegator.SpeedValueChangeEvent += SetSpeedValue;
        animationManager.AnimationDelegator.PlayIconEvent += SetPlayIconImage;
    }

    private void SetMovementName(string name)
    {
        movementTextMesh.text = name;
    }

    private void SetActionName(string name)
    {
        actionTextMesh.text = name;
    }

    private void SetSpeedValue(float speed)
    {
        if (speed >= 1.0f)
            speedValueTextMesh.text = string.Format("{0:0}", speed);
        else if (speed >= 0.5f)
            speedValueTextMesh.text = string.Format("{0:0.#}", speed);
        else if (speed >= 0.25f)
            speedValueTextMesh.text = string.Format("{0:0.##}", speed);
        else if (speed <= 0.0f)
            speedValueTextMesh.text = "0";
        else
            speedValueTextMesh.text = "error";
    }

    private void SetPlayIconImage(bool isPlaying)
    {
        if (isPlaying)
            iconImageMeshRenderer.material = playIconMaterial;
        else
            iconImageMeshRenderer.material = pauseIconMaterial;
    }

    public void CreateCommandName(string str)
    {
        GameObject g = Instantiate(ResourcePool.GetInstance().GetCommandNamePrefab(), transform);
        g.GetComponent<CommandFadeOut>().SetCommandText(str);
    }

    public void SetCommandQueue(string str)
    {
        command = str;
    }

    public void SetDebugText(string str)
    {
        debugTextMesh.text = str;
    }

	public void DisableTwoUI()
	{
		MeshRenderer[] renderers = rightUI.GetComponentsInChildren<MeshRenderer>();

		foreach (MeshRenderer renderer in renderers)
			renderer.enabled = false;
		renderers = leftUI.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers)
			renderer.enabled = false;
	}
}
