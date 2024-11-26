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


    public void StartQuiz(int index)
    {
        QuizManager.Instance.StartQuiz(instance.quizSettingsInfo[index]);
    }

    public void StartDialog(string index)
    {
        var splitted = index.Split('_');
        int dialog = int.Parse(splitted[0]);
        DialogSettings settings = dialogSettingsInfo[dialog];
        DialogManager.Instance.StartDialog(settings);
        if(splitted.Length > 1)
            NPCManager.Instance.TalkNPC(settings, splitted[1]);
    }

    public void EndQuiz()
    {
        QuizManager.Instance.EndQuiz();
    }

    public void EndDialog()
    {
        DialogManager.Instance.EndDialog();
    }

    public void MoveNPC(NPCMovement movement)
    {
        NPCManager.Instance.MoveNPC(movement);
    }

}
