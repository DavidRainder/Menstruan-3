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
    private Animator[] _interactItemsAnimators;

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
        for(int i = 0; i < _interactItems.Length; i++)
        {
            if (_interactItems[i] != null && _interactItems[i] != item)
            {
                _interactItems[i].enabled = enable;
                _interactItemsAnimators[i].SetBool("Blocked", !enable);
            }
            else if(_interactItems[i] != null)
            {
                _interactItemsAnimators[i].SetBool("Interact", !enable);
            }
        }
    }

    public void EndItem(InteractItem item)
    {
        if (!_grifo.IsNameZoneFree(-1))
        {
            Debug.Log("Grifo: " + item.itemHasToWash());
            _interactItemsAnimators[item.GetIndex()].SetBool("Correct", item.itemHasToWash());
        }
        else
        {
            Debug.Log("Basura: " + !item.itemHasToWash());
            _interactItemsAnimators[item.GetIndex()].SetBool("Correct", !item.itemHasToWash());
        }
        _interactItemsAnimators[item.GetIndex()].SetBool("End", true);

        _grifo.ResetValues();
        _trash.ResetValues();
        for(int i = 0;i < _dropZones.Length; ++i)
        {
            _dropZones[i].gameObject.SetActive(true);
            if (item == _interactItems[i]) _interactItems[i] = null;
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

        _interactItems[itemIndex].AfterInteract();
    }

    public void Collided(int index, bool enable)
    {
        _interactItemsAnimators[index].SetBool("Collided", enable);
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
