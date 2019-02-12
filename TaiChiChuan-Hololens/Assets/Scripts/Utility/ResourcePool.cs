using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class ResourcePool
{
    static private ResourcePool instance = null;

    // Parameters in Settings.txt
    private int totalFrameNumber;
    private int lastFrameNumber;

    private List<TaichiMovement> taichiMovementArray = new List<TaichiMovement>();
    public List<TaichiMovement> TaichiMovementArray { get { return taichiMovementArray; } }


    // Load Prefab
    private int teachAssistantCount = 0;
    private GameObject userInterfacePrefab;
    private GameObject commandNamePrefab;


    static public ResourcePool GetInstance()
    {
        if (instance == null)
            instance = new ResourcePool();

        return instance;
    }

    private ResourcePool()
    {
        ReadSettings();
        ReadTaichiMovement();

        userInterfacePrefab = Resources.Load<GameObject>("Prefabs/UserInterface");
        commandNamePrefab = Resources.Load<GameObject>("Prefabs/CommandName");
    }

    private void ReadSettings()
    {
        TextAsset text = Resources.Load("settings") as TextAsset;
        string[] lines = text.ToString().Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // 1st parameter.
        totalFrameNumber = System.Convert.ToInt32(lines[0].Split(' ')[1]);
        // 2nd parameter.
        lastFrameNumber = System.Convert.ToInt32(lines[1].Split(' ')[1]);
    }

    private void ReadTaichiMovement()
    {
        int lastMovementNumber = 0;
        List<TaichiAction> taichiActionArray = new List<TaichiAction>();

        TextAsset text = Resources.Load("taichi_frame_number") as TextAsset;
        string[] lines = text.ToString().Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] words = line.Split(',');
            int currentMovementNumber = System.Convert.ToInt32(words[0]);
            int actionNumber = System.Convert.ToInt32(words[1]);
            int frameNumberBegin = System.Convert.ToInt32(words[4]);

            if (currentMovementNumber != lastMovementNumber)
            {
                TaichiMovement taichiMovement = new TaichiMovement(
                    currentMovementNumber,
                    1.0f * frameNumberBegin / totalFrameNumber,
                    words[2],
                    Resources.Load<AudioClip>("Sound/movement_sound/" + words[2]));

                if (taichiMovementArray.Count > 0)
                {
                    taichiMovementArray[taichiMovementArray.Count - 1].SetTaichiActionArray(taichiActionArray);
                    taichiActionArray = new List<TaichiAction>();
                }
                taichiMovementArray.Add(taichiMovement);
                lastMovementNumber = currentMovementNumber;
            }

            TaichiAction taichiAction = new TaichiAction(
                actionNumber,
                1.0f * frameNumberBegin / totalFrameNumber,
                words[3],
                Resources.Load<AudioClip>("Sound/action_sound/" + words[3]));

            taichiActionArray.Add(taichiAction);
        }

        taichiMovementArray[taichiMovementArray.Count - 1].SetTaichiActionArray(taichiActionArray);


        // Also set normalized end time in movement and action.
        for (int i = 0; i < taichiMovementArray.Count; ++i)
        {
            if (i != taichiMovementArray.Count - 1)
            {
                taichiMovementArray[i].NormalizedEndTime = taichiMovementArray[i + 1].NormalizedBeginTime;
            }
            else
            {
                taichiMovementArray[i].NormalizedEndTime = 1.0f * lastFrameNumber / totalFrameNumber;
            }

            List<TaichiAction> currentActionArray = taichiMovementArray[i].TaichiActionArray;
            for (int j = 0; j < currentActionArray.Count; ++j)
            {
                if (j != currentActionArray.Count - 1)
                    currentActionArray[j].NormalizedEndTime = currentActionArray[j + 1].NormalizedBeginTime;
                else
                    currentActionArray[j].NormalizedEndTime = taichiMovementArray[i].NormalizedEndTime;
            }
        }
    }

    public GameObject GetAvatarsPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Avatars");
    }

    public GameObject GetUserInterfacePrefab()
    {
        return userInterfacePrefab;
    }

    public GameObject GetCommandNamePrefab()
    {
        return commandNamePrefab;
    }

    public GameObject GetCarpetPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Carpet");
    }

    public GameObject GetCoachModelPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/TaichiCoach");
    }

    public GameObject GetTeachAssistantModelPrefab()
    {
        GameObject gameObject = null;
        // Male Model
        if (teachAssistantCount % 2 == 0)
        {
            int count = teachAssistantCount / 2;
            teachAssistantCount++;
            gameObject = Resources.Load<GameObject>("Prefabs/AvatarsModels/MaleAvatar_" + (count % 4).ToString());
        }
        // Female Model
        else
        {
            int count = teachAssistantCount / 2;
            teachAssistantCount++;
            gameObject = Resources.Load<GameObject>("Prefabs/AvatarsModels/FemaleAvatar_" + (count % 4).ToString());
        }

        return gameObject;
    }

    public GameObject GetMirrorPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Mirror");
    }

    public GameObject GetFlatFootprintPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/FlatFootprintController");
    }

    public GameObject GetCircleRipplePrefab()
    {
        return Resources.Load<GameObject>("Prefabs/CircleRippleController");
    }

    public GameObject GetBalanceBarPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/BalanceBarController");
    }

    public GameObject GetLevel1ControlPanelPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Level1ControlPanel");
    }

    public GameObject GetLevel2ControlPanelPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/Level2ControlPanel");
    }

	public GameObject GetSystemSettingControlPanelPrefab()
	{
		return Resources.Load<GameObject>("Prefabs/SystemSettingControlPanel");
	}

	public GameObject GetDescriptionControlPanelPrefab()
	{
		return Resources.Load<GameObject>("Prefabs/DescriptionControlPanel");
	}

	public GameObject GetSingleModeControlPanelPrefab()
	{
		return Resources.Load<GameObject>("Prefabs/SingleModeControlPanel");
	}

	public GameObject GetMirrorControlPanelPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/MirrorControlPanel");
    }

    public GameObject GetWeightHintMoreControlPanelPrefab()
    {
        return Resources.Load<GameObject>("Prefabs/WeightHintMoreControlPanel");
    }

    public GameObject GetWaitHintCapsulePrefab()
    {
        return Resources.Load<GameObject>("Prefabs/WaitHintCapsule");
    }
}
