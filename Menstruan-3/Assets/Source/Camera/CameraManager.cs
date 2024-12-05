using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Singleton

    private static CameraManager instance = null;
    public static CameraManager Instance { get { return instance; } }

    private Camera _camera;

    enum CameraAnimationStates
    {
        ZOOM_OUT,
        MOVE, 
        ZOOM_IN,
        NUM_STATES
    }

    private CameraAnimationStates _state;

    [SerializeField]
    private float _zPositionZoomOut, _zPositionZoomIn, _lerpFactor;

    private float _threshold = 0.05f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _camera = Camera.main;
            _state = CameraAnimationStates.ZOOM_OUT;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    public void MoveToSprite(CamMovements movement)
    {
        StartCoroutine(CameraMovement(movement));
    }

    IEnumerator CameraMovement(CamMovements movement)
    {
        movement.onStart.Invoke();
        _state = CameraAnimationStates.ZOOM_OUT;
        while(_state != CameraAnimationStates.NUM_STATES)
        {
            switch (_state)
            {
                case CameraAnimationStates.MOVE:
                    if (Move(movement)) _state++;
                    break;
                case CameraAnimationStates.ZOOM_OUT:
                    if (ZoomState(_zPositionZoomOut)) _state++;
                    break;
                case CameraAnimationStates.ZOOM_IN:
                    if (ZoomState(_zPositionZoomIn)) _state++;
                    break;
            }
            yield return new WaitForEndOfFrame();
        }
        movement.onEnd.Invoke();
    }

    private bool ZoomState(float lastPos)
    {
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, lastPos, _lerpFactor * Time.deltaTime);
        return Mathf.Abs(lastPos - _camera.orthographicSize) < _threshold;
    }

    private bool Move(CamMovements movement)
    {
        Vector3 lastPos = new Vector3(movement.lastPosition.x, movement.lastPosition.y, _camera.transform.position.z);
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, lastPos, _lerpFactor * Time.deltaTime);
        Vector3 dist = movement.lastPosition - _camera.transform.position;

        return Mathf.Pow(dist.x, 2) + Mathf.Pow(dist.y, 2) < Mathf.Pow(_threshold, 2);
    }
}
