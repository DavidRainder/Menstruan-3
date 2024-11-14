using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu]
public class QuizSettings : ScriptableObject
{
    public string question;
    public string[] wrongOptions;
    public string rightOption;

    [Header("Events")]
    public UnityEvent onCorrectOption;
    public UnityEvent onWrongOption;
}

public class QuizManager : MonoBehaviour
{
    [SerializeField]
    private float _offsetQuestionButtons;
    [SerializeField]
    private Image _questionImage;
    [SerializeField]
    private TextMeshProUGUI _questionText;
    [SerializeField]
    private VerticalLayoutGroup _layout;

    QuizSettings _settings;

    public void SetInfo(QuizSettings settings)
    {
        _settings = settings;

        _questionText.text = settings.question;
        
        int correctAnswer = UnityEngine.Random.Range(0, settings.wrongOptions.Length);
        Button rightButton = _layout.transform.GetChild(correctAnswer / 2).transform.GetChild(correctAnswer % 2).GetComponent<Button>();
        TextMeshProUGUI rightText = rightButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        rightText.text = settings.rightOption;
        rightButton.gameObject.SetActive(true);
        

        int wrongSize = settings.wrongOptions.GetLength(0);
        Button[] wrongButtons = new Button[wrongSize];
        for(int i = 1; i <= wrongSize; i++) {
            int index = (correctAnswer + i) % (wrongSize + 1);
            GameObject g = _layout.transform.GetChild(index / 2).transform.GetChild(index % 2).gameObject;
            wrongButtons[i - 1] = g.GetComponent<Button>();
            TextMeshProUGUI wrongText = g.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            wrongText.text = settings.wrongOptions[i - 1];
            g.SetActive(true);
        }

        for (int i = wrongSize + 1; i < 4; ++i)
        {
            _layout.transform.GetChild(i / 2).transform.GetChild(i % 2).gameObject.SetActive(false);
        }

        gameObject.GetComponent<VerticalLayoutGroup>().spacing = _offsetQuestionButtons;
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());

        rightButton.onClick.AddListener(RightOptionChoosed);

        for(int j = 0; j < wrongSize; ++j)
        {
            wrongButtons[j].onClick.AddListener(WrongOptionChoosed);
        }
    }

    void RightOptionChoosed()
    {
        _settings.onCorrectOption.Invoke();
    }

    void WrongOptionChoosed()
    {
        _settings.onWrongOption.Invoke();
    }

    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }
}
