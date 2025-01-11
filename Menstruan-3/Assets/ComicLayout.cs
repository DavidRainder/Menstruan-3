using System;
using UnityEngine;
using UnityEngine.Events;

public class ComicLayout : MonoBehaviour
{
    #region Singleton

    private static ComicLayout instance = null;
    public static ComicLayout Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField] bool _DEBUG_MODE = false;

    /// <summary>
    /// 0 -> Normal
    /// 1 -> Inclined
    /// 2 -> Surprised
    /// 3 -> Background
    /// </summary>
    [SerializeField] Sprite[] _frames;

    public enum FRAME_TYPE
    {
        NORMAL = 0,
        INCLINED = 1,
        SURPRISED = 2,


        BACKGROUND = 3
    }

    [Serializable]
    public struct ComicStrip
    {
        public Sprite _image;
        public FRAME_TYPE _type;
        public UnityEvent onStart;
        public UnityEvent onEnd;
        public bool _moveToNext;
    }

    [Serializable]
    public struct Comic
    {
        public ComicStrip[] _strips;
    }

    [SerializeField] Comic _layout;

    [SerializeField] int _numCols;

    [SerializeField] Vector3 _initialPosition;

    private UnityAction MoveCam(int a)
    {
        return delegate { GameManager.MoveCamera(a); };
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject lastGO = null;
        int numRows = _layout._strips.Length / _numCols + _layout._strips.Length % _numCols;
        for (int i = 0; i < _layout._strips.Length; ++i) {
            CamMovements movement = new CamMovements();
            movement.onStart = _layout._strips[i].onStart;
            movement.onEnd = _layout._strips[i].onEnd;

            if (_layout._strips[i]._moveToNext)
            {
                if (i != _layout._strips.Length - 1)
                {
                    UnityAction action = MoveCam(i + 1);
                    movement.onEnd.AddListener(action);
                }
            }

            GameObject obj = new GameObject("ComicStrip_" + i);
            obj.AddComponent<SpriteRenderer>().sprite = _layout._strips[i]._image;
            if(!lastGO)
            {
                obj.transform.position = _initialPosition;
            }
            else if (i % _numCols != 0)
            {
                obj.transform.position = lastGO.transform.position + Vector3.right * 
                    lastGO.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            }
            else
            {
                obj.transform.position = lastGO.transform.position + Vector3.down * 
                    lastGO.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                obj.transform.position = Vector3.right *_initialPosition.x + Vector3.up * obj.transform.position.y + Vector3.forward * obj.transform.position.z;
            }
            GameObject frame = new GameObject("ComicFrame_" + i);
            SpriteRenderer rend = frame.AddComponent<SpriteRenderer>();
            rend.sprite = _frames[(int)_layout._strips[i]._type];
            rend.sortingOrder = obj.GetComponent<SpriteRenderer>().sortingOrder + 1;
            frame.transform.position = obj.transform.position;
            lastGO = obj;
            movement.lastPosition = obj.transform.position;
            GameManager.AddCameraMovement(movement);
        }

        GameObject background = new GameObject("ComicBackground");
        SpriteRenderer b = background.AddComponent<SpriteRenderer>();
        b.sprite = _frames[(int)FRAME_TYPE.BACKGROUND];
        b.sortingOrder = lastGO.GetComponent<SpriteRenderer>().sortingOrder - 1;
        background.transform.localScale = new Vector3(numRows * background.transform.localScale.x,
            _numCols * background.transform.localScale.y, 0);
        background.transform.position = _initialPosition + new Vector3(b.bounds.size.x / background.transform.localScale.x,
            -b.bounds.size.y / background.transform.localScale.y, 0);

        if (_DEBUG_MODE)
        {
            GameManager.MoveCamera(_layout._strips.Length - 1);
        }
    }
}