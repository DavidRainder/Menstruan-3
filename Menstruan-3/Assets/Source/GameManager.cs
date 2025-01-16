using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField]
    private GameObject _endGameObject = null;

    [SerializeField]
    private QuizSettings[] quizSettingsInfo;

    [SerializeField]
    private DialogSettings[] dialogSettingsInfo;

    [SerializeField]
    private List<CamMovements> cameraPositions = new List<CamMovements>();

    [SerializeField]
    private GameObject[] minigamesPrefabs;

    private bool mute = false;

    private MuteButton button;

    public static void Quit()
    {
        Application.Quit();
    }
    public static void StartQuiz(int index)
    {
        QuizManager.Instance.StartQuiz(instance.quizSettingsInfo[index]);
    }

    public void StartDialog(string index)
    {
        var splitted = index.Split('_');
        int dialog = int.Parse(splitted[0]);
        DialogSettings settings = instance.dialogSettingsInfo[dialog];
        if(splitted.Length > 1)
        {
            NPCManager.Instance.TalkNPC(splitted[1]);
            DialogManager.Instance.SetNPC(splitted[1]);
        }
        else
        {
            DialogManager.Instance.SetNPC("");
        }
        DialogManager.Instance.StartDialog(settings);
    }

    public static void EndQuiz()
    {
        QuizManager.Instance.EndQuiz();
    }

    public static void EndDialog()
    {
        DialogManager.Instance.EndDialog();
    }

    public static void MoveNPC(NPCMovement movement)
    {
        NPCManager.Instance.MoveNPC(movement);
    }

    public static void StopTalking(string id)
    {
        NPCManager.Instance.StopNPC(id);
    }

    public static void StartAnimation(string id)
    {
        AnimationManager.Instance.StartAnimation(id);
    }

    public static void StopAnimation()
    {
        AnimationManager.Instance.EndAnimation();
    }

    public static void AddCameraMovement(CamMovements movements)
    {
        instance.cameraPositions.Add(movements);
    }

    public static void MoveCamera(int id)
    {
        CameraManager.Instance.MoveToSprite(instance.cameraPositions[id]);
    }

    public static void TPCamera(int id)
    {
        CameraManager.Instance.TPToSprite(instance.cameraPositions[id]);
    }

    public static void StartMinigame(int id)
    {
        MinigameInstanceManager.Instance.StartMinigame(instance.minigamesPrefabs[id]);
    }

    public static void StartMinigameNoAnimation(int id)
    {
        MinigameInstanceManager.Instance.StartMinigameNoAnimation(instance.minigamesPrefabs[id]);
    }

    public static void EndMinigame(string dialogID)
    {
        MinigameInstanceManager.Instance.EndMinigame();
        instance.StartCoroutine(instance.AfterMinigame(dialogID));
    }

    public static void StartMusic(float fadeInTime)
    {
        MusicManager.Instance.StartMusic(fadeInTime);
    }

    public static void StopMusic(float fadeOutTime)
    {
        MusicManager.Instance.StopMusic(fadeOutTime);
    }

    public void EndGame()
    {
        Transform a = Instantiate(Instance._endGameObject, Camera.main.transform.position, Quaternion.identity).transform;
        a.position = new Vector3(a.position.x, a.position.y, 0);
    }

    public void ChangeMute()
    {
        Instance.mute = !Instance.mute;
        SetMute();
    }

    public void SetMute()
    {
        Instance.button?.onMuteClick(Instance.mute);
        MusicManager.Instance.Mute(Instance.mute);
        FMODUnity.RuntimeManager.MuteAllEvents(Instance.mute);
    }

    public void DisableButtonMute()
    {
        Instance.button = null;
    }

    public bool IsMute()
    {
        return mute;
    }

    public void RegisterMuteButton(MuteButton mButton)
    {
        button = mButton;
    }

    IEnumerator AfterMinigame(string dialogID)
    {
        while (MinigameInstanceManager.Instance.IsPlayingAnimation("BackToDialoguesScene"))
        {
            yield return new WaitForEndOfFrame();
        }

        MinigameInstanceManager.Instance.ResetAnim();
        StartDialog(dialogID);
    }
}
