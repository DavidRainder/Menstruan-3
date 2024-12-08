using FMOD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


public class ShowDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _endTextPointer;

    private bool _textEnded;

    DialogSettings _settings;

    int _currentText;

    DialogueSounds _sound;

    Transform _currentSoundTransform;

    public void SetSettings(DialogSettings settings, Transform soundTransform) { this._settings = settings; _currentSoundTransform = soundTransform; }

    private void Start()
    {
        _sound = gameObject.GetComponent<DialogueSounds>();
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

        if(Input.GetKeyDown(KeyCode.Space)) { 
            StopAllCoroutines();
            EndDialog();
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
        _sound.SetSoundTransform(_currentSoundTransform);
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
                _sound.playLetterSound(currentLetter);

                yield return new WaitForSeconds(_settings.speed);
            }
        }
        _textEnded = true;
        _endTextPointer.SetActive(_textEnded);
    }
}