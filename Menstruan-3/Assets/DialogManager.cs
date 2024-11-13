using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
[CreateAssetMenu]
public class DialogSettings : ScriptableObject
{
    public List<string> texts;

    public float speed;

    [Header("Events")]
    public UnityEvent onStartDialog;
    public UnityEvent onFinishDialog;
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
        if(instance._dialogInstance != null)
        {
            Destroy(instance._dialogInstance);
        }

        instance._dialogInstance = Instantiate(dialogPrefab, instance.canvas.transform);
        ShowDialog dialog = instance._dialogInstance.GetComponent<ShowDialog>();
        dialog.SetSettings(settings);
    }
}
