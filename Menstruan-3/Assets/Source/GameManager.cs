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
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField]
    private QuizSettings[] quizSettingsInfo;

    [SerializeField]
    private DialogSettings[] dialogSettingsInfo;


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
}
