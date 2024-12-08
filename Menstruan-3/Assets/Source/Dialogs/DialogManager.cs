using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            npcs = new Dictionary<string, Transform>();
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField] private GameObject dialogPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Dictionary<string, Transform> npcs;

    private Transform currentSoundPos = null;

    private GameObject _dialogInstance;

    public void RegisterNPCTransform(string npc, Transform transform)
    {
        npcs[npc] = transform;
    }

    public void StartDialog(DialogSettings settings)
    {
        instance.StartCoroutine(instance.StartDialogCoroutine(settings));
    }

    public void EndDialog()
    {
        Destroy(instance._dialogInstance);
    }
    
    public void SetNPC(string npc)
    {
        if (npcs.ContainsKey(npc))
        {
            currentSoundPos = npcs[npc];
        }
        else 
        {
            currentSoundPos = null;
        }
    }

    IEnumerator StartDialogCoroutine(DialogSettings settings)
    {
        while (StringManager.Instance.IsChangingLanguage())
        {
            yield return null;
        }

        if (instance._dialogInstance != null)
        {
            Destroy(instance._dialogInstance);
        }

        instance._dialogInstance = Instantiate(dialogPrefab, instance.canvas.transform);
        ShowDialog dialog = instance._dialogInstance.GetComponent<ShowDialog>();
        dialog.SetSettings(settings, currentSoundPos);
    }
}
