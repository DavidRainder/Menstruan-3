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

    [SerializeField]
    List<GameObject> _dropZones;

    bool _gameFinished;

    void StartMinigame()
    {
        _gameFinished = false;
        onStart.Invoke();
    }

    void EndMinigame()
    {
        onEnd.Invoke();
    }

    public void comprobar()
    {
        _gameFinished = true;
        int i = 0;
        while ( i < _dropZones.Count && _gameFinished)
        {
            DropZoneComponent dropZonComp = _dropZones[i].GetComponent<DropZoneComponent>();

            _gameFinished = dropZonComp.IsCorrect();
            i++;
        }

        Debug.Log("Juego Correcto: " + _gameFinished);
    }

}
