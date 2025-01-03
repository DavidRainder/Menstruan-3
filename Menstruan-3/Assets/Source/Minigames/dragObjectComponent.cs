using System.Collections;
using System.Drawing;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class DragObjectComponent : MonoBehaviour
{
    private Transform _myTransform;
    private Vector3 _offset;
    private Vector3 _initialPos;
    private Vector3 _dropPos;
    bool _inDropZone;
    bool _stop;
    bool _isDragging;
    private BoxCollider2D _collider;

    [SerializeField]
    private bool _alwaysDrop = false;

    [SerializeField]
    bool _onlyIfIndex = false;

    InfoTypeComponent _myInfoTypeComponent;

    private FMOD.Studio.EventInstance _dropSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _stop = false;
        _dropSound = FMODUnity.RuntimeManager.CreateInstance("event:/PickUpMinigame");
        _collider = GetComponent<BoxCollider2D>();
        _myInfoTypeComponent = GetComponent<InfoTypeComponent>();
        _myTransform = transform;
        _initialPos = _myTransform.position;
        _inDropZone = false;
        _isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stop)
        {
            Debug.Log("PARA");
            _myTransform.position = _initialPos;
        }
        else if (_isDragging)
        {
            Debug.Log("ESTOY DRAGGING");
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;

        }
    }

    private void OnMouseDown()
    {
        _stop = false;
        _inDropZone = false;
        _dropSound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _dropSound.setParameterByName("Dropped", 0);
        _offset = _myTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isDragging = true;
        _dropSound.start();
    }

    private void OnMouseUp()
    {
        if (!_stop)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_myTransform.position, _collider.bounds.size, _myTransform.eulerAngles.z);

            int i = 0;
            while (i < colliders.Length)
            {
                // Si es una zone donde se puede colocar
                DropZoneComponent dzComp = colliders[i].GetComponent<DropZoneComponent>();
                if (dzComp != null && dzComp.enabled)
                {

                    // Si no esta ocupado
                    if(_myInfoTypeComponent != null)
                    {
                        int indx = _myInfoTypeComponent.GetIndex();
                        if (!_onlyIfIndex)
                        {
                            if ((((int)_myInfoTypeComponent.GetInfoType() == 0) && dzComp.IsNameZoneFree(indx)) || (((int)_myInfoTypeComponent.GetInfoType() == 1) && dzComp.IsDescriptionZoneFree(indx)))
                            {
                                _dropSound.setParameterByName("Dropped", 1);
                                Vector3 pos = dzComp.GetZonePosition((int)(_myInfoTypeComponent.GetInfoType()));
                                _inDropZone = true;
                                _myTransform.position = pos;
                            }
                        }
                        else if(indx == dzComp.GetIndex())
                        {
                            _dropSound.setParameterByName("Dropped", 1);
                            _inDropZone = true;
                        }

                    }
                    else
                    {
                        _dropSound.setParameterByName("Dropped", 1);
                        _inDropZone = true;
                    }
                }
                i++;
            }

            if (!_inDropZone && !_alwaysDrop)
                _myTransform.position = _initialPos;

            _isDragging = false;
            _stop = false;
        }
    }
    
    private void OnDestroy()
    {
        _dropSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public bool IsInDropZone() { return _inDropZone; }

    public void StopDrag()
    {
        _stop = true;
    }

    public void SetInitialPos()
    {
        _initialPos = transform.position;
    }
}
