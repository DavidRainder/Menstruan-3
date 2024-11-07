using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DialogSettings
{
    public List<string> texts;

    public float speed;
}

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
    [SerializeField] private Canvas canvas;

    private GameObject _dialogInstance;

    public void StartDialog(DialogSettings settings)
    {
        if(_dialogInstance != null)
        {
            Destroy(_dialogInstance);
        }

        _dialogInstance = Instantiate(dialogPrefab, canvas.transform);
        ShowDialog dialog = _dialogInstance.GetComponent<ShowDialog>();
        dialog.SetSettings(settings);
        
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
