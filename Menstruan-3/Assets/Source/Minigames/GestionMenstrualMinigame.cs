using System.Collections;
using UnityEngine;
public class GestionMenstrualMinigame : MonoBehaviour
{
    [SerializeField]
    private DropZoneComponent _grifo, _trash;

    [SerializeField]
    private DropZoneComponent[] _dropZones;

    [SerializeField]
    private InteractItem[] _interactItems;

    [SerializeField]
    private Animator _clockAnimation;

    private MinigameManager _minigameManager;

    private int _cont;

    public void EnableDragToBody(int index, bool enable)
    {
        foreach (DropZoneComponent drop in _dropZones)
        {
            if(enable && drop.GetIndex() == index) drop.gameObject.SetActive(true);
            else drop.gameObject.SetActive(false);
        }
    }

    public void EnableFinalDrag(bool enable)
    {
        _grifo.gameObject.SetActive(enable);
        _trash.gameObject.SetActive(enable);
    }

    public void EnableItems(InteractItem item, bool enable)
    {
        foreach (InteractItem item2 in _interactItems)
        {
            if (item2 != item) item2.enabled = enable;
        }
    }

    public void EndItem()
    {
        _grifo.ResetValues();
        _trash.ResetValues();
        foreach (DropZoneComponent drop in _dropZones)
        {
            drop.gameObject.SetActive(true);
        }
        EnableItems(null, true);
        _cont++;
    }

    public void SetIndex(int index)
    {
        _grifo.SetIndex(index);
        _trash.SetIndex(index);
    }

    public void StartClockAnim(int itemIndex)
    {
        _clockAnimation.enabled = true;
        _clockAnimation.Rebind();
        _clockAnimation.Update(0f);
        StartCoroutine(WaitClock(itemIndex));
    }

    IEnumerator WaitClock(int itemIndex)
    {
        while (_clockAnimation.GetCurrentAnimatorStateInfo(0).IsName("Clock"))
        {
            yield return new WaitForEndOfFrame();
        }

        int i = 0;
        while(i < _interactItems.Length && _interactItems[i].GetIndex() != itemIndex) ++i;
        if(i < _interactItems.Length)
        {
            _interactItems[i].AfterInteract();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cont = 0;        
        _minigameManager = gameObject.GetComponent<MinigameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cont == _interactItems.Length)
        {
            _minigameManager.EndMinigame();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<MinigameManager>().EndMinigame();
        }
    }
}
