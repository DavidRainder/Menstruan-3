using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;

public class ShowDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _shownText;

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
        if (_textEnded && _currentText < _settings.texts.Count && Input.GetMouseButtonUp(0))
        {
            ShowText();
        }
    }

    public void ShowText()
    {
        int initialIndex = 0;
        StartCoroutine(AnimText(initialIndex, _settings.texts[_currentText++]));
    }

    IEnumerator AnimText(int initialIndex, string messageToShow)
    {
        _textEnded = false;
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
    }
}
