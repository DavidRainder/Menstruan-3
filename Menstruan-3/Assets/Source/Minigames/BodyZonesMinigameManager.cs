using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BodyZonesMinigameManager : MonoBehaviour
{
    bool _gameFinished = false;

    [SerializeField]
    List<GameObject> _dropZones;

    MinigameManager _myMinigameManager;
    BodyMinigameUI_Manager _myMinigameUIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _myMinigameManager = GetComponent<MinigameManager>();
        _myMinigameUIManager = GetComponent<BodyMinigameUI_Manager>();
        _myMinigameManager.StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            _myMinigameManager.EndMinigame();
        }
    }

    public void Comprobar()
    {
        bool description = false;
        bool name = false;

        int individualCorrects = 0;
        int fullCorrects = 0;

        int i = 0;
        while (i < _dropZones.Count)
        {
            DropZoneComponent dropZonComp = _dropZones[i].GetComponent<DropZoneComponent>();

            description = dropZonComp.IsDescriptionCorrect();
            name = dropZonComp.IsNameCorrect();

            if (name && description)
            {
                fullCorrects++;
                individualCorrects += 2;
            }
            else if (name || description)
                individualCorrects++;


            i++;
        }

        // Actualizo UI
        _myMinigameUIManager.Comprobar();
        _myMinigameUIManager.SetCorrects(individualCorrects);
        _myMinigameUIManager.SetIncorrects((_dropZones.Count * 2 - individualCorrects));

        _gameFinished = (fullCorrects == _dropZones.Count);
        Debug.Log("Juego Correcto: " + _gameFinished);

        if (_gameFinished)
        {
            _myMinigameManager.EndMinigame();
        }
    }
}
