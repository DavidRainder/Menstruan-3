using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField]
    private QuizSettings[] quizSettingsInfo;

    [SerializeField]
    private DialogSettings[] dialogSettingsInfo;

    [SerializeField]
    private CamMovements[] cameraPositions;

    [SerializeField]
    private GameObject[] minigamesPrefabs;


    public static void StartQuiz(int index)
    {
        QuizManager.Instance.StartQuiz(instance.quizSettingsInfo[index]);
    }

    public static void StartDialog(string index)
    {
        var splitted = index.Split('_');
        int dialog = int.Parse(splitted[0]);
        DialogSettings settings = instance.dialogSettingsInfo[dialog];
        DialogManager.Instance.StartDialog(settings);
        if(splitted.Length > 1)
            NPCManager.Instance.TalkNPC(splitted[1]);
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
