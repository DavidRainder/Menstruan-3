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
