using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BodyZonesMinigameManager : MonoBehaviour
{
    bool _gameFinished = false;

    [SerializeField]
    List<GameObject> _dropZones;

    [SerializeField]
    GameObject _correctTextGO;
    TextMeshProUGUI _correctText;
    [SerializeField]
    GameObject _incorrectTextGO;
    TextMeshProUGUI _incorrectText;


    MinigameManager _myMinigameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _myMinigameManager = GetComponent<MinigameManager>();
        _myMinigameManager.StartMinigame();

        _correctText = _correctTextGO.GetComponent<TextMeshProUGUI>();
        _incorrectText = _incorrectTextGO.GetComponent<TextMeshProUGUI>();

        Debug.Log("NULO: " + (_correctText == null) + "NULO2: " + (_incorrectText == null));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void comprobar()
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

        _correctText.text = "Bien: " + individualCorrects;
        _incorrectText.text = "Mal: " + (_dropZones.Count * 2 - individualCorrects);

        _gameFinished = (fullCorrects == _dropZones.Count);
        Debug.Log("Juego Correcto: " + _gameFinished);

        if (_gameFinished)
        {
            _myMinigameManager.EndMinigame();
        }
    }
}
