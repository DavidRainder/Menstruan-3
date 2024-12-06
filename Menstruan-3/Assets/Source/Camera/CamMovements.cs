using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[CreateAssetMenu]
public class CamMovements : ScriptableObject
{
    [Header("Events")]
    public UnityEvent onStart;
    public UnityEvent onEnd;

    public Vector3 lastPosition;

    public CamMovements()
    {
        onStart = new UnityEvent();
        onEnd = new UnityEvent();

        lastPosition = Vector3.zero;
    }
}
