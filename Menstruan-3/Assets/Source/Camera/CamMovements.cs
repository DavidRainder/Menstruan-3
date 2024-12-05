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
}
