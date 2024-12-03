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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Holah?");
        InfoTypeComponent itComp = collision.gameObject.GetComponent<InfoTypeComponent>();
        if (itComp != null)
        {
            if ((int)itComp.GetInfoType() == 0) // Nombre
            {
                _nameOccupied = true;
                _nameCorrect = (_index == itComp.GetIndex());
                Debug.Log("Nombre puesto");
            }
            else if ((int)itComp.GetInfoType() == 1) // Descripcion
            {
                _descriptionOccupied = true;
                _descriptionCorrect = (_index == itComp.GetIndex());
                Debug.Log("Descripcion puesta");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        InfoTypeComponent itComp = collision.gameObject.GetComponent<InfoTypeComponent>();
        if (itComp != null)
        {
            if ((int)itComp.GetInfoType() == 0) // Nombre
            {
                Debug.Log("Nombre kitado");
                _nameOccupied = false;
                _nameCorrect = false;
            }
            else if ((int)itComp.GetInfoType() == 1) // Descripcion
            {
                Debug.Log("Descripcion kitado");
                _descriptionOccupied = false;
                _descriptionCorrect = false;
            }
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("Holah?");
    //    InfoTypeComponent itComp = collision.gameObject.GetComponent<InfoTypeComponent>();
    //    if (itComp != null)
    //    {
    //        if((int)itComp.GetInfoType() == 0) // Nombre
    //        {
    //            _nameOccupied = true;
    //            _nameCorrect = (_index == itComp.GetIndex());
    //            Debug.Log("Nombre puesto");
    //        }
    //        else if((int)itComp.GetInfoType() == 1) // Descripcion
    //        {
    //            _descriptionOccupied = true;
    //            _descriptionCorrect = (_index == itComp.GetIndex());
    //            Debug.Log("Descripcion puesta");
    //        }
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    InfoTypeComponent itComp = collision.gameObject.GetComponent<InfoTypeComponent>();
    //    if (itComp != null)
    //    {
    //        if ((int)itComp.GetInfoType() == 0) // Nombre
    //        {
    //            Debug.Log("Nombre kitado");
    //            _nameOccupied = false;
    //            _nameCorrect = false;
    //        }
    //        else if ((int)itComp.GetInfoType() == 1) // Descripcion
    //        {
    //            Debug.Log("Descripcion kitado");
    //            _descriptionOccupied = false;
    //            _descriptionCorrect = false;
    //        }
    //    }
    //}
}
