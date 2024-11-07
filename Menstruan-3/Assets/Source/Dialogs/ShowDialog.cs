using NUnit.Framework;
using System.Collections;
using TMPro;
using UnityEngine;

public class ShowDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _shownText;
    [SerializeField] private float _timeToShowNextCharacter;

    private bool _textEnded;

    private void Start()
    {
        ShowText();
    }

    private void OnDestroy()
    {
        StopCoroutine("AnimText");
    }

    public void ShowText()
    {
        int initialIndex = 0;
        StartCoroutine(AnimText(initialIndex, _shownText));
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

                yield return new WaitForSeconds(_timeToShowNextCharacter);
            }
        }
        _textEnded = true;
    }
}
