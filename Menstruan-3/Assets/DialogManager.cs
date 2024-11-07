using UnityEngine;

public class DialogManager : MonoBehaviour
{

    #region Singleton

    private static DialogManager instance = null;
    public static DialogManager Instance {get { return instance; }}

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField] private GameObject dialogPrefab;

    private GameObject _dialogInstance;

    void StartDialog()
    {
        if(_dialogInstance != null)
        {
            Destroy(_dialogInstance);
        }

        _dialogInstance = Instantiate(dialogPrefab);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
