using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMode : MonoBehaviour {

	private TextMesh stageTextMesh;
	private GameObject Text;
	private GameObject Bar;
	private float DIS2STAGE = 1.8f;
	public string MovementName;
	public string ActionName;

	// Use this for initialization
	void Start () {
		stageTextMesh = transform.Find("Text").GetComponent<TextMesh>();
		Text = GameObject.Find("Text");
		Bar = GameObject.Find("MenuBar");
	}
	
	// Update is called once per frame
	void Update () {
		SetStageText();
		transform.position = Camera.main.transform.position + DIS2STAGE * Camera.main.transform.forward;
		transform.forward = Camera.main.transform.forward;
	}

	private void SetStageText()
	{
		Director director = GameObject.Find("Director").GetComponent<Director>();

		//set stage Text when not in the MainPage
		if (director.stageCode[director.stageCode.Count - 1] != 0)
		{
			MeshRenderer[] renderers = Text.transform.GetComponentsInChildren<MeshRenderer>();

			foreach (MeshRenderer renderer in renderers)
				renderer.enabled = true;
			renderers = Bar.transform.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer renderer in renderers)
				renderer.enabled = true;
		}
		else
		{
			MeshRenderer[] renderers = Text.transform.GetComponentsInChildren<MeshRenderer>();

			foreach (MeshRenderer renderer in renderers)
				renderer.enabled = false;
			renderers = Bar.transform.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer renderer in renderers)
				renderer.enabled = false;
		}

		//handling stage num >= 2
		if (director.stageCode.Count >= 3)
			stageTextMesh.text = "<color=#817B7BFF>" + "<< " + director.stage[director.stageCode[director.stageCode.Count - 2]] + "</color>" + " → " + director.stage[director.stageCode[director.stageCode.Count - 1]];
		else if (director.stageCode.Count == 2)
			stageTextMesh.text = "<color=#817B7BFF>" + director.stage[director.stageCode[director.stageCode.Count - 2]] + "</color>" + " → " + director.stage[director.stageCode[director.stageCode.Count - 1]];
		else
			stageTextMesh.text = director.stage[director.stageCode[director.stageCode.Count - 1]];

		//show MovementName and ActionName in Practicing 
		if (director.seriesMode || director.singleMode)
		{
			if (director.stageCode[director.stageCode.Count - 1] == 1 || (director.stageCode[director.stageCode.Count - 1] == 2 && !director.IsUsingControlPanel) || (director.stageCode[director.stageCode.Count - 1] == 2 && director.IsUsingHelpInSingleMode))
			{
				stageTextMesh.text += "\n" + "招式名稱：" + MovementName;
			}
			else if (director.stageCode[director.stageCode.Count - 1] == 3)
			{
				stageTextMesh.text += "\n" + "招式名稱：" + MovementName + "\n" + "分解動作：" + ActionName;
			}
		}

		//for Debug
		//stageTextMesh.text += "\n" + director.stageCode[director.stageCode.Count - 1];
	}
}
