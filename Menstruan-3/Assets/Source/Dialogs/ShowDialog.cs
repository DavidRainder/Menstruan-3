using FMOD;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ShowDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _endTextPointer;

    private bool _textEnded;

    DialogSettings _settings;

    int _currentText;
    public void SetSettings(DialogSettings settings) { this._settings = settings; }

    FMOD.System _system;

    private int _maxLetters = 0;
    private string _path = Application.dataPath + "/Sounds/Letters/";
    private Dictionary<string, Sound> _soundsDict;

    private void Start()
    {
        _system = FMODUnity.RuntimeManager.CoreSystem;
        _soundsDict = new Dictionary<string, Sound>();
        DirectoryInfo info = new DirectoryInfo(_path);
        FileInfo[] files = info.GetFiles();
        foreach(FileInfo file in files)
        {
            Sound sound;
            string name = file.Name.Split(".")[0];
            if (!_soundsDict.ContainsKey(name))
            {
                _system.createSound(file.Name, MODE._2D | MODE.LOOP_OFF | MODE.CREATESAMPLE | MODE.LOWMEM, out sound);
                _soundsDict.Add(name, sound);
                _maxLetters++;
            }
        }

        ShowText();
    }

    private void OnDestroy()
    {
        StopCoroutine("AnimText");
    }

    public void Update()
    {
        if (_textEnded && Input.GetMouseButtonUp(0))
        {
            ShowText();
        }
    }

    private void StartDialog()
    {
        _settings.onStartDialog.Invoke();
    }

    private void EndDialog()
    {
        _settings.onFinishDialog.Invoke();
        Destroy(gameObject);
    }

    public void ShowText()
    {
        if(_currentText == 0)
        {
            StartDialog();
        }
        if(_currentText >= _settings.texts.Count)
        {
            EndDialog();
        }
        else
        {
            int initialIndex = 0;
            StartCoroutine(AnimText(initialIndex, _settings.texts[_currentText++]));
        }
    }

    IEnumerator AnimText(int initialIndex, string messageToShow)
    {
        _system.getMasterChannelGroup(out ChannelGroup channelgroup);

        _textEnded = false;
        _endTextPointer.SetActive(_textEnded);
        messageToShow = StringManager.Instance.GetGenderStringByKey(messageToShow);

        if (initialIndex < messageToShow.Length && initialIndex >= 0)
        {
            int index = initialIndex;
            int length = messageToShow.Length;

            while (++index <= length)
            {
                string dialog = messageToShow.Substring(initialIndex, index - initialIndex);
                _text.text = dialog;
                char currentLetter = dialog[dialog.Length - 1];

                if(_soundsDict.ContainsKey(currentLetter.ToString()))
                    _system.playSound(_soundsDict[currentLetter.ToString()], channelgroup, false, out Channel channel);

                yield return new WaitForSeconds(_settings.speed);
            }
        }
        _textEnded = true;
        _endTextPointer.SetActive(_textEnded);
    }
}