using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class PhasesMinigame : MonoBehaviour
{
    [SerializeField]
    List<DropZoneComponent> _dropZones = new List<DropZoneComponent>();

    [SerializeField]
    List<DragObjectComponent> _draggedObjects = new List<DragObjectComponent>();

    [SerializeField]
    Button _cluesButton = null;

    [SerializeField]
    Button _checkButton = null;

    int _timesChecked = 0;

    private void Start()
    {
        GetComponent<MinigameManager>().StartMinigame();
    }

    public void Check()
    {
        bool checking = true;
        foreach(DropZoneComponent zone in _dropZones)
        {
            if(!(zone.IsOccupied() &&
                zone.GetDraggedObject().GetComponent<DropZoneIndex>().GetIndex() == zone.GetIndex()))
            {
                checking = false;
                break;
            }
        }

        if(checking)
        {
            // Esto funciona
            Debug.Log("GANASTE AAAAAAAAAAAAAAAAAAAAAAAAAA");
            if(FMODEventEmitter.Instance != null) {
                FMODEventEmitter.Instance.EmitEvent("CorrectAnswer");
            }
            GetComponent<MinigameManager>().EndMinigame();
        }
        else
        {
            if (FMODEventEmitter.Instance != null)
            {
                FMODEventEmitter.Instance.EmitEvent("WrongAnswer");
            }
            _cluesButton.interactable = ++_timesChecked >= 3;
        }
    }

    public void SortRandomlly()
    {
        List<DropZoneComponent> tempZones = new List<DropZoneComponent>(_dropZones);
        foreach (DragObjectComponent drag in  _draggedObjects)
        {
            int ind = Random.Range(0, tempZones.Count);
            if (tempZones.Count == _dropZones.Count && tempZones[ind].GetIndex() == drag.GetComponent<DropZoneIndex>().GetIndex())
            {
                ind = Random.Range(ind + 1, ind + tempZones.Count) % tempZones.Count;
            }
            drag.SetPosition(tempZones[ind].transform.position);
            tempZones.RemoveAt(ind);
        }
    }
    
    // Toni ya sé que se podría hacer mejor.
    // Me debes un café si estás viendo esto porque hay mejores
    // cosas (y más útiles) que hacer que revisar código
    // de Juegos Serios.
    //
    // Fdo. David Rivera Martínez
    private void Update()
    {
        bool active = true;
        foreach (DropZoneComponent zone in _dropZones)
        {
            active = zone.IsOccupied();
            if (!active) break;
        }
        _checkButton.interactable = active;
    }

    public void GiveFeedback()
    {
        bool check = true;
        foreach (DropZoneComponent zone in _dropZones)
        {
            DragObjectComponent drag = zone.GetDraggedObject();
            if (!(zone.IsOccupied() &&
                drag.GetComponent<DropZoneIndex>().GetIndex() == zone.GetIndex()))
            {
                check = false;
                drag.GetComponent<Animator>().SetTrigger("Wrong");
            }
            else
            {
                drag.GetComponent<Animator>().SetTrigger("Correct");
            }
        }
        if (!check && FMODEventEmitter.Instance != null)
        {
            FMODEventEmitter.Instance.EmitEvent("WrongAnswer");
        }
    }
}
