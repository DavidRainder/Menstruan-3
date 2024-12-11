using NUnit.Framework;
using System;
using UnityEngine;

public class InteractItem : MonoBehaviour
{
    [SerializeField]
    private string _animationBeforeParameterName;

    [SerializeField]
    private string _animationInteractParameterName;

    private Animator _animator;

    private DragObjectComponent _drag;

    [SerializeField]
    private bool _ignoreNoDrop = false;

    [SerializeField]
    private GestionMenstrualMinigame _minigame;

    public enum InteractStates
    {
        BEFORE_INTERACT,
        DRAG,
        INTERACT,
        AFTER_INTERACT
    }

    private InteractStates _interactStates;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _drag = GetComponent<DragObjectComponent>();
        _drag.enabled = false;
        _interactStates = InteractStates.BEFORE_INTERACT;
        _minigame = transform.GetComponentInParent<GestionMenstrualMinigame>();
        Debug.Assert(_minigame != null);
    }

    private void OnMouseUp()
    {
        if(_interactStates == InteractStates.BEFORE_INTERACT)
        {
            Debug.Log("CAMBIO ESTADO A DRAG");
            //_animator.SetBool(_animationBeforeParameterName, true);
            _interactStates++;
            _drag.enabled = true;
            _minigame.EnableDragToBody(this, true);
        }
        else if(_interactStates == InteractStates.DRAG && _drag.IsInDropZone())
        {
            Debug.Log("CAMBIO ESTADO A INTERACT");

            _interactStates++;
            _drag.SetInitialPos();
            _drag.enabled = false;
        }
        else if(_interactStates == InteractStates.INTERACT)
        {
            Debug.Log("CAMBIO ESTADO A AFTER");
            //_animator.SetBool(_animationInteractParameterName, true);
            _interactStates++;
            _drag.enabled = true;
            _minigame.EnableDragToBody(this, false);
        }
        else if (_interactStates == InteractStates.AFTER_INTERACT && _drag.IsInDropZone())
        {
            Debug.Log("TERMINE");

            _interactStates++;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_ignoreNoDrop)
        {
            NoDropComponent noDrop = collision.GetComponent<NoDropComponent>();
            if (noDrop != null && _interactStates != InteractStates.BEFORE_INTERACT)
            {
                Debug.Log("STOP DRAG");
                _drag.StopDrag();
            }
        }
    }
}
