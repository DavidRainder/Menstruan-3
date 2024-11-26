using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[CreateAssetMenu]
public class QuizSettings : ScriptableObject
{
    public string question;
    public string[] wrongOptions;
    public string rightOption;

    [Header("Events")]
    public UnityEvent onCorrectOption;
    public UnityEvent onWrongOption;
}
