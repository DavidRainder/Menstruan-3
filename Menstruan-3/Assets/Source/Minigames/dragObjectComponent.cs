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

    InfoTypeComponent _myInfoTypeComponent;

    private FMOD.Studio.EventInstance _dropSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dropSound = FMODUnity.RuntimeManager.CreateInstance("event:/PickUpMinigame");
        _myRenderer = GetComponent<SpriteRenderer>();
        _myInfoTypeComponent = GetComponent<InfoTypeComponent>();
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
        _dropSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _dropSound.setParameterByName("Dropped", 0);
        _offset = _myTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isDragging = true;
        _dropSound.start();
    }

    private void OnMouseUp()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_myTransform.position, _myRenderer.bounds.size, _myTransform.eulerAngles.z);

        int i = 0;
        while (i < colliders.Length)
        {
            // Si es una zone donde se puede colocar
            DropZoneComponent dzComp = colliders[i].GetComponent<DropZoneComponent>();
            if (dzComp != null)
            {

                // Si no esta ocupado
                int indx = _myInfoTypeComponent.GetIndex();
                if ((((int)_myInfoTypeComponent.GetInfoType() == 0) && dzComp.IsNameZoneFree(indx)) || (((int)_myInfoTypeComponent.GetInfoType() == 1) && dzComp.IsDescriptionZoneFree(indx)))
                {
                    _dropSound.setParameterByName("Dropped", 1);
                    Vector3 pos = dzComp.GetZonePosition((int)(_myInfoTypeComponent.GetInfoType()));
                    _inDropZone = true;
                    _myTransform.position = pos;
                }
            }
            i++;
        }

        if (!_inDropZone)
            _myTransform.position = _initialPos;

        _inDropZone = false;
        _isDragging = false;
    }

    private void OnDestroy()
    {
        _dropSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
