using System.Collections.Generic;
using UnityEngine;

public class DropZoneComponent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    bool _nameOccupied;
    bool _descriptionOccupied;

    bool _nameCorrect;
    bool _descriptionCorrect;

    [SerializeField]
    private int _index;

    [SerializeField]
    List<GameObject> _dropZonesPartes;

    private void Start()
    {
        _nameOccupied = false;
        _descriptionOccupied = false;
        _nameCorrect = false;
        _descriptionCorrect = false;
    }
    public Vector3 GetZonePosition(int i)
    {
        return _dropZonesPartes[i].transform.position;
    }


    public bool IsCorrect()
    {
        return _descriptionCorrect && _nameCorrect;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        InfoTypeComponent itComp = collision.gameObject.GetComponent<InfoTypeComponent>();
        if (itComp != null)
        {
            if((int)itComp.GetInfoType() == 0) // Nombre
            {
                _nameOccupied = true;
                _nameCorrect = (_index == itComp.GetIndex());
            }
            else if((int)itComp.GetInfoType() == 1) // Descripcion
            {
                _descriptionOccupied = true;
                _descriptionCorrect = (_index == itComp.GetIndex());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        InfoTypeComponent itComp = collision.gameObject.GetComponent<InfoTypeComponent>();
        if (itComp != null)
        {
            if ((int)itComp.GetInfoType() == 0) // Nombre
            {
                _nameOccupied = false;
                _nameCorrect = false;
            }
            else if ((int)itComp.GetInfoType() == 1) // Descripcion
            {
                _descriptionOccupied = false;
                _descriptionCorrect = false;
            }
        }
    }
}
