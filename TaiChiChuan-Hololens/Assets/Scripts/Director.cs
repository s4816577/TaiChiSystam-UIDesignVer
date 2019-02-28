using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{
    private AudioSource audioSource;

    private AvatarsController avatarsController;
    private List<IAvatar> avatarList;
    private UserInterface userInterface;
    private AnimationManager animationManager;
    public AnimationManager AnimationManager { get { return animationManager; } }
    private IPlaybackState playbackState;
    public IPlaybackState PlaybackState { get { return playbackState; } }
    private AdditionalHint additionalHint;
    public AdditionalHint AdditionalHint { get { return additionalHint; } }

    private UnityEngine.VR.WSA.Input.GestureRecognizer gestureRecognizer;
    private UnityEngine.VR.WSA.Input.GestureRecognizer navigationRecognizer;
    private Vector3 navigationStartPosition;
    private GameObject controlPanel;
    public GameObject ControlPanel { get { return controlPanel; } }

	private int count = 0;
	public float LastAvatarsHeight = -0.34f;
	public bool IsUsingControlPanel = true;

	public string[] stage = {"楊氏太極拳", "套路模式", "單招模式", "分解動作", "系統設定", "指令說明", "調整高度", "調整速度", "單招動作"};
	public List<int> stageCode;
	public bool seriesMode = false;
	public bool singleMode = false;
	private bool showingPause = true;
	private bool showingPlay = true;
	private bool IsRecording = false;
	public bool IsUsingHelpInSingleMode = false;

	private FileWriter fileWriter;

	// Use this for initialization
	void Start()
    {
		//Add init stage and get file writer
		stageCode.Add(0);
		GameObject g = GameObject.Find("FileWriter");
		fileWriter = g.GetComponent<FileWriter>();

        // For playing Sound.
        audioSource = this.gameObject.AddComponent<AudioSource>();

        // AvatarsController
        avatarsController = AvatarsController.GetInstance();
        avatarList = new List<IAvatar>();
        foreach (IAvatar avatar in avatarsController.Coaches)
            avatarList.Add(avatar);
        avatarList.Add(avatarsController.ReferenceCoach);
        foreach (IAvatar avatar in avatarsController.TeachAssistants)
            avatarList.Add(avatar);

        // UserInterface
        userInterface = UserInterface.GetInstance();

        //CreateContinuousModeAnimationManager();
        CreateDetailedModeAnimationManager();
        //playbackState.Restart();

        //additionalHint = new MirrorAdditionalHint(avatarsController);
        //additionalHint = new GroundingTeachAssistantAdditionalHint(avatarsController);
        additionalHint = new NormalTeachAssistantAdditionalHint(avatarsController);

		//set controlPanel to modify restartInd in AnimationManager.cs
		SetLevel1ControlPanel();

		// Gesture Reconizer for capture "Tap"
		gestureRecognizer = new UnityEngine.VR.WSA.Input.GestureRecognizer();
        gestureRecognizer.SetRecognizableGestures(UnityEngine.VR.WSA.Input.GestureSettings.Tap);
        gestureRecognizer.TappedEvent += (UnityEngine.VR.WSA.Input.InteractionSourceKind source, int tapCount, Ray ray) =>
        {
			if (!IsUsingControlPanel)
			{
				if (seriesMode)
				{
					if (stageCode[stageCode.Count - 1] == 1)
					{
						count++;

						if (count == 1)
						{
							Invoke("Replay", 1f);
						}
						else if (count >= 2)
						{
							CancelInvoke("Replay");
							GoBackMainMenu();
						}
						/*else if (count >= 3)
						{
							CancelInvoke("GoToDetailSingleMode");
							CancelInvoke("GoBackMainMenu");
							Play();							
						}*/
					}
					/*else if (stageCode[stageCode.Count - 1] == 8)
					{
						count++;

						if (count == 1)
						{
							Invoke("Next", 1f);
						}
						else if (count == 2)
						{
							CancelInvoke("Next");
							Invoke("GoBackSeriesMode", 1f);
						}
						else if (count >= 3)
						{
							CancelInvoke("GoBackSeriesMode");
							CancelInvoke("Next");
							Last();
						}
					}*/
				}
				else if (singleMode)
				{
					if (stageCode[stageCode.Count - 1] == 2)
					{
						count++;

						if (count == 1)
						{
							Invoke("GoToDetailMode", 1f);
						}
						else if (count >= 2)
						{
							CancelInvoke("GoToDetailMode");
							GoBackSingleMenu();
						}
						/*else if (count >= 3)
						{
							CancelInvoke("GoToDetailMode");
							CancelInvoke("GoBackSingleMenu");
							Play();
						}*/
					}
					else if (stageCode[stageCode.Count - 1] == 3)
					{
						count++;

						if (count == 1)
						{
							Invoke("Next", 1f);
						}
						else if (count == 2)
						{
							CancelInvoke("Next");
							Invoke("GoBackSingleMode", 1f);
						}
						else if (count >= 3)
						{
							CancelInvoke("GoBackSingleMode");
							CancelInvoke("Next");
							Last();
						}
					}
				}
			}
			else
			{
				//add BackToMenu when using 5 and 4
				if (stageCode[stageCode.Count - 1] == 5 || stageCode[stageCode.Count - 1] == 4 || (stageCode[stageCode.Count - 1] == 2 && !IsUsingHelpInSingleMode))
				{
					count++;
					if (count == 1)
					{
						Invoke("ResetCount", 1f);
					}
					else if (count >= 2)
					{
						CancelInvoke("ResetCount");
						GoBackMainMenu();
					}
				}
				else if (stageCode[stageCode.Count - 1] == 7 || stageCode[stageCode.Count - 1] == 6)
				{
					count++;
					if (count == 1)
					{
						Invoke("ResetCount", 1f);
					}
					else if (count >= 2)
					{
						CancelInvoke("ResetCount");
						GoBackSystemSettingMenu();
					}
				}
				else if (stageCode[stageCode.Count - 1] == 0)
				{
					count++;
					if (count == 1 || count == 2)
					{
						Invoke("ResetCount", 1f);
					}
					else if (count >= 3)
					{
						if (!IsRecording)
						{
							CancelInvoke("ResetCount");
							CreateNewFileAndStartRecording();
						}
						else
						{
							CancelInvoke("ResetCount");
							StopRecording();
						}
					}
				}
			}
			//if (controlPanel == null)
			//{
			//    SetLevel1ControlPanel();
			//    StopCapturingGlobalClicked();
			//}
		};

        // Gesture Reconizer for capture "Drag"
        navigationRecognizer = new UnityEngine.VR.WSA.Input.GestureRecognizer();
        navigationRecognizer.SetRecognizableGestures(UnityEngine.VR.WSA.Input.GestureSettings.NavigationY);
        navigationRecognizer.NavigationStartedEvent += (UnityEngine.VR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray) =>
        {
            gestureRecognizer.StopCapturingGestures();
        };
        navigationRecognizer.NavigationUpdatedEvent += (UnityEngine.VR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray) =>
        {
            avatarsController.AvatarsHeight = avatarsController.AvatarsBaseHeight + relativePosition.y * 0.2f;
        };
        navigationRecognizer.NavigationCompletedEvent += (UnityEngine.VR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray) =>
        {
            navigationRecognizer.StopCapturingGestures();
            gestureRecognizer.StartCapturingGestures();
        };
        navigationRecognizer.NavigationCanceledEvent += (UnityEngine.VR.WSA.Input.InteractionSourceKind source, Vector3 relativePosition, Ray ray) =>
        {
            navigationRecognizer.StopCapturingGestures();
            gestureRecognizer.StartCapturingGestures();
        };

        // Start looking for "Tap" gestures.
        gestureRecognizer.StartCapturingGestures();
		UnActiveAvatars();
	}

    public void StartCapturingGlobalClicked()
    {
        if (!gestureRecognizer.IsCapturingGestures())
            gestureRecognizer.StartCapturingGestures();
    }

    public void StopCapturingGlobalClicked()
    {
        if (gestureRecognizer.IsCapturingGestures())
            gestureRecognizer.StopCapturingGestures();
    }

    // TODO Rename method.
    public void StartCapturingNavigation()
    {
        userInterface.CreateCommandName("Height");
        if (!navigationRecognizer.IsCapturingGestures())
            navigationRecognizer.StartCapturingGestures();
    }

    private void CreateContinuousModeAnimationManager()
    {
        animationManager = new ContinuousModeAnimationManager(this, avatarList, audioSource, avatarsController);

        playbackState = new NormalPlaybackState(this, animationManager);
        playbackState.Pause();

        userInterface.SetAnimationManagerMode(animationManager);
        userInterface.ShowRightUI(false);
    }

    private void CreateDetailedModeAnimationManager()
    {
        animationManager = new DetailedModeAnimationManager(this, avatarList, audioSource, avatarsController);

        playbackState = new NormalPlaybackState(this, animationManager);
        playbackState.Pause();

		//userInterface.SetAnimationManagerMode(animationManager);
		//userInterface.ShowRightUI(true);
		userInterface.DisableTwoUI();
    }

    public void SetPlaybackState(IPlaybackState playbackState)
    {
        this.playbackState = playbackState;
    }

    // Update is called once per frame
    private void Update()
    {
        animationManager.Update();
        playbackState.Update();
    }

    private void OnApplicationQuit()
    {
        additionalHint.Dispose();
    }

    // For speech input handler.
    public void Play()
    {
		if (!IsUsingControlPanel && stageCode[stageCode.Count - 1] != 3 && stageCode[stageCode.Count - 1] != 8)
		{
			if (showingPlay)
			{
				SaveInformation("觸發Play");
				userInterface.CreateCommandName("Play");
			}
			else
			{
				showingPlay = true;
			}
			playbackState.Play();
			count = 0;
		}
    }

	//It's equal to Hint
	public void Help()
	{
		if (!IsUsingControlPanel)
		{
			SaveInformation("觸發Help");
			userInterface.CreateCommandName("Help");
			SetHelpControlPanel();
			count = 0;
		}
	}

	public void Pause()
    {
		if (!IsUsingControlPanel && stageCode[stageCode.Count - 1] != 3 && stageCode[stageCode.Count - 1] != 8)
		{
			if (showingPause)
			{
				SaveInformation("觸發Stop");
				userInterface.CreateCommandName("Stop");
			}
			else
			{
				showingPause = true;
			}
			playbackState.Pause();
		}
    }

    public void Restart()
    {
		if (!IsUsingControlPanel)
		{
			SaveInformation("觸發Restart");
			userInterface.CreateCommandName("Restart");
			playbackState.Restart();
			Followed();

			if (additionalHint is MirrorAdditionalHint)
			{
				SetMirrorAdditionalHint();
			}
		}
    }

    public void Freeze()
    {
		if (!IsUsingControlPanel)
		{
			SaveInformation("觸發Freeze");
			userInterface.CreateCommandName("Freeze");
			avatarsController.SetCoachPositionMode(new FreezedCoachPositionMode());
			playbackState.Pause();
		}
    }

    public void ResetFollowed()
    {
		if (!IsUsingControlPanel)
		{
			SaveInformation("觸發Reset");
			userInterface.CreateCommandName("Reset");
			Followed();
			playbackState.Pause();

			if (additionalHint is MirrorAdditionalHint)
			{
				SetMirrorAdditionalHint();
			}
		}
    }

    private void Followed()
    {
        // Adjust avatars group.
        Vector3 cameraHorizontalRotation = Camera.main.transform.rotation.eulerAngles;
        cameraHorizontalRotation.x = cameraHorizontalRotation.z = 0.0f;
        avatarsController.SetCoachPositionMode(new FollowedCoachPositionMode(Quaternion.Euler(cameraHorizontalRotation)));

        // Adjust single rotation.
        CoachAvatar referenceCoach = avatarsController.ReferenceCoach;
        foreach (CoachAvatar coachAvatar in avatarsController.Coaches)
        {
            coachAvatar.ResetRotation(referenceCoach.HeadHorizontalRotation());
        }
        foreach (TeachAssistantAvatar teachAssistantAvatar in avatarsController.TeachAssistants)
        {
            teachAssistantAvatar.ResetRotation(referenceCoach.HeadHorizontalRotation());
        }
    }

    public void CountinuousMode()
    {
        userInterface.CreateCommandName("Continuous Mode");
        CreateContinuousModeAnimationManager();
    }

    public void DetailedMode()
    {
        userInterface.CreateCommandName("Segmented Mode");
        CreateDetailedModeAnimationManager();
    }

    public void Next()
    {
		if (!IsUsingControlPanel && stageCode[stageCode.Count - 1] != 2)
		{
			SaveInformation("觸發Next");
			userInterface.CreateCommandName("Next");
			playbackState.Next();
			count = 0;
		}
    }

    public void Last()
    {
		if (!IsUsingControlPanel && stageCode[stageCode.Count - 1] != 2)
		{
			SaveInformation("觸發Last");
			userInterface.CreateCommandName("Last");
			playbackState.Last();
			count = 0;
		}
    }

    public void NextMovement()
    {
        userInterface.CreateCommandName("Next Movement");
        playbackState.NextMovement();
    }

    public void LastMovement()
    {
        userInterface.CreateCommandName("Last Movement");
        playbackState.LastMovement();
    }

    public void NextAction()
    {
        userInterface.CreateCommandName("Next Action");
        playbackState.NextAction();
    }

    public void LastAction()
    {
        userInterface.CreateCommandName("Last Action");
        playbackState.LastAction();
    }

    public void Speedup()
    {
        userInterface.CreateCommandName("Speed Up");
        playbackState.SpeedUp();
    }

    public void Speeddown()
    {
        userInterface.CreateCommandName("Speed Down");
        playbackState.SpeedDown();
    }

    public void SetNormalAdditionalHint()
    {
        if (additionalHint != null)
            additionalHint.Dispose();
        additionalHint = new NormalTeachAssistantAdditionalHint(avatarsController);

        avatarsController.Coaches[0].Avatar.transform.eulerAngles = avatarsController.ReferenceCoach.Avatar.transform.eulerAngles;

        Animator firstCoachAnimator = avatarsController.Coaches[0].Avatar.gameObject.GetComponent<Animator>();
        firstCoachAnimator.runtimeAnimatorController = avatarsController.ReferenceCoach.Avatar.gameObject.GetComponent<Animator>().runtimeAnimatorController;
        animationManager.UpdateFirstCoachAnamator(firstCoachAnimator);
    }

    public void SetGroundingInterfaceAdditionalHint()
    {
        if (additionalHint != null)
            additionalHint.Dispose();
        additionalHint = new GroundingTeachAssistantAdditionalHint(avatarsController);

        avatarsController.Coaches[0].Avatar.transform.eulerAngles = avatarsController.ReferenceCoach.Avatar.transform.eulerAngles;

        Animator firstCoachAnimator = avatarsController.Coaches[0].Avatar.gameObject.GetComponent<Animator>();
        firstCoachAnimator.runtimeAnimatorController = avatarsController.ReferenceCoach.Avatar.gameObject.GetComponent<Animator>().runtimeAnimatorController;
        animationManager.UpdateFirstCoachAnamator(firstCoachAnimator);
    }

    public void SetRippleInterfaceAdditionalHint()
    {
        if (additionalHint != null)
            additionalHint.Dispose();
        additionalHint = new RippleTeachAssistantAdditionalHint(avatarsController);

        avatarsController.Coaches[0].Avatar.transform.eulerAngles = avatarsController.ReferenceCoach.Avatar.transform.eulerAngles;

        Animator firstCoachAnimator = avatarsController.Coaches[0].Avatar.gameObject.GetComponent<Animator>();
        firstCoachAnimator.runtimeAnimatorController = avatarsController.ReferenceCoach.Avatar.gameObject.GetComponent<Animator>().runtimeAnimatorController;
        animationManager.UpdateFirstCoachAnamator(firstCoachAnimator);
    }

    public void SetScrollbarInterfaceAdditionalHint()
    {
        if (additionalHint != null)
            additionalHint.Dispose();
        additionalHint = new ScrollbarTeachAssistantAdditionalHint(avatarsController);

        avatarsController.Coaches[0].Avatar.transform.eulerAngles = avatarsController.ReferenceCoach.Avatar.transform.eulerAngles;

        Animator firstCoachAnimator = avatarsController.Coaches[0].Avatar.gameObject.GetComponent<Animator>();
        firstCoachAnimator.runtimeAnimatorController = avatarsController.ReferenceCoach.Avatar.gameObject.GetComponent<Animator>().runtimeAnimatorController;
        animationManager.UpdateFirstCoachAnamator(firstCoachAnimator);
    }

    public void SetMirrorAdditionalHint()
    {
        if (additionalHint != null)
            additionalHint.Dispose();
        additionalHint = new MirrorAdditionalHint(avatarsController);

        avatarsController.Coaches[0].Avatar.transform.eulerAngles = avatarsController.ReferenceCoach.Avatar.transform.eulerAngles + new Vector3(0, 180, 0);

        Animator firstCoachAnimator = avatarsController.Coaches[0].Avatar.gameObject.GetComponent<Animator>();
        firstCoachAnimator.runtimeAnimatorController = Resources.Load("mirrored") as RuntimeAnimatorController;
        animationManager.UpdateFirstCoachAnamator(firstCoachAnimator);
    }

    public void DestroyControlPanel()
    {
        if (controlPanel != null)
            UnityEngine.Object.Destroy(controlPanel);
    }

	public void DestroyHelpControlPanel()
	{
		SaveInformation("退出幫助頁面");
		count = 0;
		if (controlPanel != null)
			UnityEngine.Object.Destroy(controlPanel);
		DisableUsingControlPanel();
		IsUsingHelpInSingleMode = false;
	}

	public void SetLevel1ControlPanel()
    {
		IsUsingControlPanel = true;
		//userInterface.CreateCommandName("Menu");
		DestroyControlPanel();
		UnActiveAvatars();
        controlPanel = Level1ControlPanel.InstantiateGameObject();
    }

    public void SetLevel2ControlPanel()
    {
        DestroyControlPanel();
        controlPanel = Level2ControlPanel.InstantiateGameObject();
    }

	public void SetSystemSettingControlPanel()
	{
		IsUsingControlPanel = true;
		//userInterface.CreateCommandName("Menu");
		DestroyControlPanel();
		UnActiveAvatars();
		controlPanel = SystemSettingControlPanel.InstantiateGameObject();
	}

	public void SetDescriptionControlPanel()
	{
		SaveInformation("進入指令說明頁面");
		IsUsingControlPanel = true;
		//userInterface.CreateCommandName("Menu");
		DestroyControlPanel();
		UnActiveAvatars();
		controlPanel = DescriptionControlPanel.InstantiateGameObject();
	}

	public void SetSingleModeControlPanel()
	{
		IsUsingControlPanel = true;
		DestroyControlPanel();
		UnActiveAvatars();
		controlPanel = SingleModeControlPanel.InstantiateGameObject();
	}

	public void SetSpeedControlPanel()
	{
		SaveInformation("進入速度調整頁面");
		IsUsingControlPanel = true;
		DestroyControlPanel();
		UnActiveAvatars();
		count = 0;
		controlPanel = SpeedControlPanel.InstantiateGameObject();
	}

	public void SetHeightControlPanel()
	{
		SaveInformation("進入高度調整頁面");
		IsUsingControlPanel = true;
		DestroyControlPanel();
		UnActiveAvatars();
		count = 0;
		controlPanel = HeightControlPanel.InstantiateGameObject();
	}

	public void SetHelpControlPanel()
	{
		SaveInformation("進入幫助頁面");
		IsUsingControlPanel = true;
		IsUsingHelpInSingleMode = true;
		controlPanel = HelpControlPanel.InstantiateGameObject(stageCode[stageCode.Count - 1]);
	}

	public void SetMirrorControlPanel()
    {
        DestroyControlPanel();
        controlPanel = MirrorControlPanel.InstantiateGameObject();
    }

    public void SetWeightHintMoreControlPanel()
    {
        DestroyControlPanel();
        controlPanel = WeightHintMoreControlPanel.InstantiateGameObject();
    }

	public void SetRestartInd(int Ind)
	{
		if (stageCode[stageCode.Count - 1] == 2)
		{
			SaveInformation("選擇第" + (Ind+1).ToString() + "招，並進入單招模式");
		}
		DisableUsingControlPanel();
		//userInterface.CreateCommandName("Change Movement");
		ActiveAvatars();
		avatarsController.SetToFront();
		playbackState.SetRestartInd(Ind);
		count = 0;
		playbackState.PlayAndSound(Ind);
		//Invoke("DisableUsingControlPanel", 2f);
	}
	/*
	public void UnUsingControlPanel()
	{
		Invoke("DisableUsingControlPanel", 2f);
	}*/

	private void DisableUsingControlPanel()
	{
		IsUsingControlPanel = false;
	}

	private void GoBackSeriesMode()
	{
		SaveInformation("回到套路模式");
		NotShowingPauseLog();
		stageCode.RemoveAt(stageCode.Count - 1);
		Pause();
		playbackState.SetRestartInd(0);
		count = 0;
	}

	private void GoBackSingleMode()
	{
		SaveInformation("回到單招模式");
		//NotShowingPauseLog();
		stageCode.RemoveAt(stageCode.Count - 1);
		//Pause();
		playbackState.SetRestartInd(animationManager.restartInd);
		playbackState.PlayAndSound(animationManager.restartInd);
		count = 0;
	}

	private void GoToDetailMode()
	{
		PlaySoundOfDetailedMode();
		SaveInformation("進入分解動作");
		NotShowingPauseLog();
		Pause();
		//userInterface.CreateCommandName("Segmented Mode");
		stageCode.Add(3);
		count = 0;
	}

	private void GoToDetailSingleMode()
	{
		SaveInformation("進入單招動作");
		NotShowingPauseLog();
		Pause();
		//userInterface.CreateCommandName("Segmented Mode");
		stageCode.Add(8);
		count = 0;
	}

	private void GoBackMainMenu()
	{
		SaveInformation("回到主頁面");
		NotShowingPauseLog();
		Pause();
		stageCode.RemoveAt(stageCode.Count - 1);
		SetLevel1ControlPanel();
		count = 0;
		if (singleMode)
			singleMode = false;
		if (seriesMode)
			seriesMode = false;
	}

	private void GoBackSingleMenu()
	{
		SaveInformation("回到單招招式選擇頁面");
		NotShowingPauseLog();
		Pause();
		//no need to pop
		//stageCode.RemoveAt(stageCode.Count - 1);
		SetSingleModeControlPanel();
		PlaySoundOfSingleMode();
		count = 0;
	}

	private void GoBackSystemSettingMenu()
	{
		SaveInformation("回到系統設定頁面");
		stageCode.RemoveAt(stageCode.Count - 1);
		SetSystemSettingControlPanel();
		//Pause();
		count = 0;
	}
	/*
	public void SetBackgroundScale()
	{
		StageMode stageMode = GameObject.Find("Stage").GetComponent<StageMode>();
		Transform Button = stageMode.transform.Find("Button");
		Button.localScale = new Vector3(0.43f, 0.13f, 0.001f);
	}*/

	private void UnActiveAvatars()
	{
		avatarsController.ActiveAvatars(false);
	}

	private void ActiveAvatars()
	{
		avatarsController.ActiveAvatars(true);
	}

	public void ResetCount()
	{
		count = 0;
	}

	//for speed control panel only
	public void SetInitSpeed(float speed)
	{
		SaveInformation("教練速度，設定為" + speed.ToString());
		userInterface.CreateCommandName("SetSpeed");
		animationManager.LastSpeed = speed;
		count = 0;
	}

	public void CreateNewFileAndStartRecording()
	{
		userInterface.CreateCommandName("Create New File and Start Recording");
		fileWriter.InitRecording();
		fileWriter.SaveInformationToFile("開始記錄");
		count = 0;
		IsRecording = true;
	}

	public void StopRecording()
	{
		userInterface.CreateCommandName("Stop Recording");
		fileWriter.SaveInformationToFile("結束記錄");
		fileWriter.EndOfRecording();
		count = 0;
		IsRecording = false;
	}

	public void UnitTest()
	{
		ActiveAvatars();
		SetRestartInd(0);
		GoToDetailMode();
	}

	public void UnitTestTwo()
	{
		Replay();
	}

	public void HeightUp()
	{
		SaveInformation("教練高度，調高2.5公分");
		avatarsController.SetAvatarsHeight(LastAvatarsHeight + 0.025f);
		HeightControlPanel[] script = GameObject.FindObjectsOfType<HeightControlPanel>();
		script[0].SetCoachHeight(LastAvatarsHeight + 0.025f);
		count = 0;
		LastAvatarsHeight = LastAvatarsHeight + 0.025f;
	}

	public void HeightDown()
	{
		SaveInformation("教練高度，調低2.5公分");
		avatarsController.SetAvatarsHeight(LastAvatarsHeight - 0.025f);
		HeightControlPanel[] script = GameObject.FindObjectsOfType<HeightControlPanel>();
		script[0].SetCoachHeight(LastAvatarsHeight - 0.025f);
		count = 0;
		LastAvatarsHeight = LastAvatarsHeight - 0.025f;
	}

	public void SaveInformation(string information)
	{
		fileWriter.SaveInformationToFile(information);
	}

	//for stage change log
	public void NotShowingPauseLog()
	{
		showingPause = false;
	}

	public void NotShowingPlayLog()
	{
		showingPlay = false;
	}

	public void PlaySoundOfSeriesMode()
	{
		AudioClip audio =  Resources.Load<AudioClip>("Sound/套路模式");
		audioSource.PlayOneShot(audio);
	}

	public void PlaySoundOfSingleMode()
	{
		AudioClip audio = Resources.Load<AudioClip>("Sound/單招模式");
		audioSource.Stop();
		animationManager.ClearAudio();
		audioSource.PlayOneShot(audio);
		playbackState.Pause();
	}

	public void PlaySoundOfDetailedMode()
	{
		AudioClip audio = Resources.Load<AudioClip>("Sound/分解動作");
		audioSource.Stop();
		animationManager.ClearAudio();
		audioSource.PlayOneShot(audio);
		playbackState.Pause();
	}

	public void Replay()
	{
		SaveInformation("觸發Replay");
		userInterface.CreateCommandName("Replay");
		NotShowingPlayLog();
		animationManager.Replay();
		count = 0;
	}
}
