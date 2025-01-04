using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;

public class InteractItem : MonoBehaviour
{
    [SerializeField]
    private string _animationInteractParameterName, _animationAfterParameterName;

    [SerializeField]
    private float _timeAfterInteract;

    private Animator _animator;

    private DragObjectComponent _drag;

    [SerializeField]
    private bool _ignoreNoDrop = false;

    [SerializeField]
    private GestionMenstrualMinigame _minigame;

    private int _index;

    [SerializeField]
    private bool _toWash;

    public bool itemHasToWash() {  return _toWash; }

    public int GetIndex()
    {
        return _index;
    }

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
        _index = gameObject.GetComponent<InfoTypeComponent>().GetIndex();
    }

    private void OnMouseUp()
    {
        if (enabled)
        {
            if(_interactStates == InteractStates.BEFORE_INTERACT)
            {
                _animator.enabled = true;
                _animator.Rebind();
                _animator.Update(0f);

                _interactStates++;
                _drag.enabled = true;
                _minigame.SetIndex(_index);
                _minigame.EnableDragToBody(_index, true);
                _minigame.EnableFinalDrag(false);
                _minigame.EnableItems(this, false);
            }
            else if(_interactStates == InteractStates.DRAG && _drag.IsInDropZone())
            {
                _interactStates++;
                _drag.SetInitialPos();
                _drag.enabled = false;
                _minigame.EnableDragToBody(-1, false);
                _minigame.EnableNeedInteractAnimation(_index, true);
            }
            else if(_interactStates == InteractStates.INTERACT)
            {
                _minigame.EnableNeedInteractAnimation(_index, false);
                _animator.SetBool(_animationInteractParameterName, true);
                _interactStates++;
                StartCoroutine(ChangeAnimationAfterInteract());
            }
            else if (_interactStates == InteractStates.AFTER_INTERACT && _drag.IsInDropZone())
            {
                _interactStates++;
                StartCoroutine(EndAnimation());
                _minigame.EndItem(this);
            }
        }
    }

    IEnumerator ChangeAnimationAfterInteract()
    {
        yield return new WaitForSeconds(_timeAfterInteract);
        _minigame.StartClockAnim(GetIndex());
    }

    public void AfterInteract()
    {
        _animator.SetBool(_animationAfterParameterName, true);
        _drag.enabled = true;
        _minigame.EnableFinalDrag(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_ignoreNoDrop)
        {
            NoDropComponent noDrop = collision.GetComponent<NoDropComponent>();
            if (noDrop != null && _interactStates != InteractStates.BEFORE_INTERACT && _drag.enabled)
            {
                Debug.Log("STOP DRAG");
                _drag.StopDrag();
                _minigame.Collided(_index, true);
                StartCoroutine(DisableAnimationCollided());
            }
        }
    }

    IEnumerator DisableAnimationCollided()
    {
        yield return new WaitForSeconds(0.2f);
        _minigame.Collided(_index, false);
    }

    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }
}
