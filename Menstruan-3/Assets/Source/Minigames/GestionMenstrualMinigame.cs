using UnityEngine;
public class GestionMenstrualMinigame : MonoBehaviour
{
    [SerializeField]
    private DropZoneComponent _grifo, _trash;

    [SerializeField]
    private DropZoneComponent[] _dropZones;

    [SerializeField]
    private InteractItem[] _interactItems;

    public void EnableDragToBody(InteractItem item, bool enable)
    {
        _grifo.enabled = !enable;
        _trash.enabled = !enable;

        foreach (DropZoneComponent drop in _dropZones)
        {
            drop.enabled = enable;
        }

        foreach (InteractItem item2 in _interactItems)
        {
            if (item2 != item) item2.enabled = !enable;
        }
    }

    public void EndItem()
    {
        _grifo.ResetValues();
        _trash.ResetValues();
    }

    public void SetIndex(int index)
    {
        _grifo.SetIndex(index);
        _trash.SetIndex(index);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<MinigameManager>().EndMinigame();
        }
    }
}
