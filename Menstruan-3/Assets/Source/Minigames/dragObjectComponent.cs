using UnityEngine;

public class DragObjectComponent : MonoBehaviour
{
    private Transform _myTransform;
    private Vector3 _offset;
    private Vector3 _initialPos;
    private Vector3 _dropPos;
    bool _inDropZone;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _myTransform = transform;
        _inDropZone = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        //Debug.Log("Holaa he clicado");
        _initialPos = _myTransform.position;
        _offset = _myTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    private void OnMouseUp()
    {
        if (_inDropZone) {
            _myTransform.position = _dropPos;
        }
        else
            _myTransform.position = _initialPos;
        //Debug.Log("Holaa he dejado de clicar");
    }

    private void OnMouseDrag()
    {
        Debug.Log("JAJA");
        _myTransform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si es una zone donde se puede colocar
        if (collision.GetComponent<DropZoneComponent>() != null) { 
            _inDropZone = true;
            _dropPos = collision.transform.position;
            Debug.Log("Pos drop: " + _dropPos);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _inDropZone = false;
    }
}
