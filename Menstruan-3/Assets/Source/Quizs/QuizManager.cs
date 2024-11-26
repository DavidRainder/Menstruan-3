using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuizManager : MonoBehaviour
{
    #region Singleton

    private static QuizManager instance = null;
    public static QuizManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField] private GameObject quizPrefab;

    private GameObject _quizInstance;

    public void StartQuiz(QuizSettings settings)
    {
        if (instance._quizInstance != null)
        {
            Destroy(instance._quizInstance);
        }

        instance._quizInstance = Instantiate(quizPrefab);
        Quiz quiz = instance._quizInstance.transform.GetChild(0).GetComponent<Quiz>();
        quiz.SetInfo(settings);
    }

    public void EndQuiz()
    {
        Destroy(instance._quizInstance);
    }
}
