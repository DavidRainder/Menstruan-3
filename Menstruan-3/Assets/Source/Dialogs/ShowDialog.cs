using NUnit.Framework;
using System.Collections;
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

    private void Start()
    {
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
        _textEnded = false;
        _endTextPointer.SetActive(_textEnded);
        messageToShow = StringManager.Instance.GetGenderStringByKey(messageToShow);

        if (initialIndex < messageToShow.Length && initialIndex >= 0)
        {
            int index = initialIndex;
            int length = messageToShow.Length;

            while (++index <= length)
            {
                _text.text = messageToShow.Substring(initialIndex, index - initialIndex);

                yield return new WaitForSeconds(_settings.speed);
            }
        }
        _textEnded = true;
        _endTextPointer.SetActive(_textEnded);
    }
}