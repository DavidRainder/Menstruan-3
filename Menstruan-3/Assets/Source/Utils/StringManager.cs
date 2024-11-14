using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

// Están en el orden de los Locales. No cambiar 
public enum Gender
{
    FEM = 1,
    MASC = 2,
    NEUTRAL = 3
}

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

    [SerializeField] private LocalizedStringTable _genderTable;

    Gender _gender;

    bool _changingLanguage;
    private void Start()
    {
        instance._changingLanguage = false;
        instance._gender = Gender.NEUTRAL;
        ChangeGender(instance._gender);
    }

    public bool IsChangingLanguage()
    {
        return instance._changingLanguage;
    }
    public string GetGenderStringByKey(string key)
    {
        StringTable table = instance._genderTable.GetTable();
        StringTableEntry entry = table.GetEntry(key);
        return entry.LocalizedValue;
    }

    public void ChangeGender(Gender gender)
    {
        instance._gender = gender;
        StartCoroutine(instance.ChangeLanguage());
    }

    /// <summary>
    /// 1 = FEM
    /// 2 = MASC
    /// 3 = NEUTRAL
    /// 
    /// Cualquier otro valor será ignorado
    /// </summary>
    /// <param name="gender"></param>
    public void ChangeGender(int gender) 
    {
        if (gender < 1 || gender > 3) return;
        instance.ChangeGender((Gender)gender);
    }

    IEnumerator ChangeLanguage()
    {
        instance._changingLanguage = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)_gender];
        instance._changingLanguage = false;
    }
}
