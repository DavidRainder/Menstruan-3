using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onStart;
    public UnityEvent onEnd;

    void StartMinigame()
    {
        onStart.Invoke();
    }

    void EndMinigame()
    {
        onEnd.Invoke();
    }



}
