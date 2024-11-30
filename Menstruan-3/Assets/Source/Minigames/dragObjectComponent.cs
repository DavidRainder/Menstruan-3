using System.Drawing;
using UnityEngine;

public class DragObjectComponent : MonoBehaviour
{
    private Transform _myTransform;
    private Vector3 _offset;
    private Vector3 _initialPos;
    private Vector3 _dropPos;
    bool _inDropZone;
    bool _isDragging;
    private SpriteRenderer _myRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       _myRenderer = GetComponent<SpriteRenderer>();
        _myTransform = transform;
        _initialPos = _myTransform.position;
        _inDropZone = false;
        _isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDragging)
        {
            _myTransform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log("Holaa he clicado");
        _offset = _myTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_myTransform.position, _myRenderer.bounds.size, _myTransform.eulerAngles.z);

        int i = 0;
        while (i < colliders.Length)
        {
        // Si es una zone donde se puede colocar
            if (colliders[i].GetComponent<DropZoneComponent>() != null)
            {
                _inDropZone = true;
                _myTransform.position = colliders[i].transform.position;
                Debug.Log("Pos drop: " + _myTransform.position);
            }
            i++;
        }

        if(!_inDropZone)
            _myTransform.position = _initialPos;

        _inDropZone = false;
        _isDragging = false;
    }
}
