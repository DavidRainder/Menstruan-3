using UnityEngine;
using System.Collections.Generic;

public class BodyZonesMinigameManager : MonoBehaviour
{
    bool _gameFinished = false;

    [SerializeField]
    List<GameObject> _dropZones;

    MinigameManager _myMinigameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _myMinigameManager = GetComponent<MinigameManager>();
        _myMinigameManager.StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void comprobar()
    {
        _gameFinished = true;
        int i = 0;
        while (i < _dropZones.Count && _gameFinished)
        {
            DropZoneComponent dropZonComp = _dropZones[i].GetComponent<DropZoneComponent>();
            if (dropZonComp == null) { Debug.Log("SOOY NULOOO"); }
            _gameFinished = dropZonComp.IsCorrect();
            i++;
        }
        Debug.Log("Juego Correcto: " + _gameFinished);

        if (_gameFinished) {
            _myMinigameManager.EndMinigame();
        }
    }
}
