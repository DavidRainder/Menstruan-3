using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class TEST_StartQuiz : MonoBehaviour
{
    [SerializeField] private QuizSettings[] _settings;

    private int index = 0;

    private void Start()
    {
        QuizManager.Instance.StartQuiz(_settings[index]);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //instance.GetComponent<RectTransform>().localPosition = new Vector3(-90, 160, 0);
            ChangeQuiz();
        }
    }

    public void ChangeQuiz()
    {
        index = (index + 1) % _settings.Length;
        QuizManager.Instance.StartQuiz(_settings[index]);
    }
}
