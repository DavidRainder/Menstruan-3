using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

public class StringManager : MonoBehaviour
{
    #region Singleton

    private static StringManager instance = null;
    public static StringManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField] private LocalizedStringTable[] _stringTableCollections;

}
