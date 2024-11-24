using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class TEST_StartQuiz : MonoBehaviour
{
    [SerializeField] private QuizSettings[] _settings;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Canvas _canvas;

    private GameObject _quizInstance = null;

    private int index = 0;

    private void Start()
    {
        _quizInstance = Instantiate(_prefab);
        _quizInstance.transform.SetParent(_canvas.transform);
        _quizInstance.GetComponent<RectTransform>().position = new Vector3(200, 150, 0);
        ChangeQuiz();
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
        _quizInstance.GetComponent<QuizManager>().SetInfo(_settings[index]);
        index = (index + 1) % _settings.Length;
    }
}
