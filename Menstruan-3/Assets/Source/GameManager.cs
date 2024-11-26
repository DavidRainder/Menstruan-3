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

    public static void StartDialog(int index)
    {
        DialogManager.Instance.StartDialog(instance.dialogSettingsInfo[index]);
    }

    public static void EndQuiz()
    {
        QuizManager.Instance.EndQuiz();
    }

    public static void EndDialog()
    {
        DialogManager.Instance.EndDialog();
    }
}
