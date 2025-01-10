using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropZoneComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Si esta ocupado se pone el indice del ocupado, si no es -1
    int _nameOccupied;
    int _descriptionOccupied;

    bool _nameCorrect;
    bool _descriptionCorrect;

    [SerializeField]
    private int _index; // Index del dropZone

    [SerializeField]
    List<GameObject> _dropZonesPartes;

    private bool _occupied = false;
    private DragObjectComponent _draggedObject = null;

    public void NotifyOccupation(DragObjectComponent drag)
    {
        _occupied = true;
        _draggedObject = drag;
    }

    public bool IsOccupied() { return _occupied; }
    public DragObjectComponent GetDraggedObject() { return _draggedObject; }

    private void Start()
    {
        _nameOccupied = -1;
        _descriptionOccupied = -1;
        _nameCorrect = false;
        _descriptionCorrect = false;
    }
    public Vector3 GetZonePosition(int i)
    {
        return _dropZonesPartes[i].transform.position;
    }

    public bool IsCorrect() { return _descriptionCorrect && _nameCorrect; }

    public bool IsNameCorrect() { return _nameCorrect; }

    public bool IsDescriptionCorrect() { return _descriptionCorrect; }

    public bool IsNameZoneFree(int i) { return (_nameOccupied == -1) || (_nameOccupied == i); }

    public bool IsDescriptionZoneFree(int i) { return (_descriptionOccupied == -1) || (_descriptionOccupied == i); }

    public void ResetValues()
    {
        _nameCorrect = false;
        _descriptionCorrect = false;

        _nameOccupied = -1;
        _descriptionOccupied = -1;
    }

    public void SetIndex(int index)
    {
        _index = index;
    }

    public int GetIndex()
    {
        return _index;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Holah?");
        InfoTypeComponent itComp = collision.gameObject.GetComponent<InfoTypeComponent>();
        if (itComp != null)
        {
            if (((int)itComp.GetInfoType() == 0) && (_nameOccupied == -1)) // Nombre
            {
                _nameOccupied = itComp.GetIndex();
                _nameCorrect = (_index == _nameOccupied);
                Debug.Log("Nombre puesto!___Correcto: " + _nameCorrect);
            }
            else if (((int)itComp.GetInfoType() == 1) && (_descriptionOccupied == -1)) // Descripcion
            {
                _descriptionOccupied = itComp.GetIndex();
                _descriptionCorrect = (_index == _descriptionOccupied);
                Debug.Log("Descripcion puesta!___Correcto: " + _descriptionCorrect);
            }
        }
        else if (_dropZonesPartes.Count == 0)
        {
            DragObjectComponent dragComponent = collision.gameObject.GetComponent<DragObjectComponent>();
            if (dragComponent != null && _draggedObject == null)
            {
                _occupied = true;
                _draggedObject = dragComponent;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        InfoTypeComponent itComp = collision.gameObject.GetComponent<InfoTypeComponent>();
        if (itComp != null)
        {
            if (((int)itComp.GetInfoType() == 0) && (_nameOccupied == itComp.GetIndex())) // Nombre
            {
                _nameOccupied = -1;
                _nameCorrect = false;
                Debug.Log("Nombre kitado!___Nombre correcto: " + _nameCorrect);
            }
            else if (((int)itComp.GetInfoType() == 1) && (_descriptionOccupied == itComp.GetIndex())) // Descripcion
            {
                _descriptionOccupied = -1;
                _descriptionCorrect = false;
                Debug.Log("Descripcion kitado!___Descripción correcto: " + _descriptionCorrect);
            }
        }
        else if (_dropZonesPartes.Count == 0)
        {
            DragObjectComponent dragComponent = collision.gameObject.GetComponent<DragObjectComponent>();
            if (dragComponent != null && dragComponent == _draggedObject)
            {
                _occupied = false;
                _draggedObject = null;
            }
        }
    }
}
