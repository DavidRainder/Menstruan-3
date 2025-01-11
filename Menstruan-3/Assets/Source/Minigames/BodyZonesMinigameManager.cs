using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BodyZonesMinigameManager : MonoBehaviour
{
    bool _gameFinished = false;

    [SerializeField]
    int feedback_X = 2;
    int tries = 0;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _myMinigameManager.EndMinigame();
        }
    }

    public void Comprobar()
    {

        if (tries < feedback_X) // Para no sumar infinitamente
            tries++;

        bool correct = false;

        int individualCorrects = 0;

        int i = 0;
        while (i < _dropZones.Count)
        {

            DropZoneComponent dropZonComp = _dropZones[i].GetComponent<DropZoneComponent>();
            DragObjectComponent drag = dropZonComp.GetDraggedObject();

            if (drag != null)
            {
                if (!(dropZonComp.IsOccupied() &&
                    drag.GetComponent<DropZoneIndex>().GetIndex() == dropZonComp.GetIndex()))
                {
                    correct = false;
                    if (tries >= feedback_X)
                        drag.GetComponent<Animator>().SetTrigger("Wrong");
                }
                else
                {
                    correct = true;
                    if (tries >= feedback_X)
                        drag.GetComponent<Animator>().SetTrigger("Correct");
                }

                if (correct)
                    individualCorrects++;

            }
            i++;
        }

        // Actualizo UI
        if (tries < feedback_X)
            _myMinigameUIManager.Comprobar();
        _myMinigameUIManager.SetCorrects(individualCorrects);
        _myMinigameUIManager.SetIncorrects((_dropZones.Count - individualCorrects));

        _gameFinished = (individualCorrects == _dropZones.Count);
        Debug.Log("Juego Correcto: " + _gameFinished);

        if (_gameFinished)
        {
            _myMinigameManager.EndMinigame();
        }
    }
}
