using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
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

    private Button _rightButton;
    private Button[] _wrongButtons;
 
    QuizSettings _settings;

    bool _buttonPressed;

    public void SetInfo(QuizSettings settings)
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);

        Animator anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        anim.Rebind();
        anim.Update(0f);

        _buttonPressed = false;
        _settings = settings;

        _questionText.text = settings.question;
        
        int correctAnswer = UnityEngine.Random.Range(0, settings.wrongOptions.Length);
        _rightButton = _layout.transform.GetChild(correctAnswer / 2).transform.GetChild(correctAnswer % 2).GetComponent<Button>();
        TextMeshProUGUI rightText = _rightButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        rightText.text = settings.rightOption;
        _rightButton.gameObject.SetActive(true);


        int wrongSize = settings.wrongOptions.GetLength(0);
        _wrongButtons = new Button[wrongSize];
        for(int i = 1; i <= wrongSize; i++) {
            int index = (correctAnswer + i) % (wrongSize + 1);
            GameObject g = _layout.transform.GetChild(index / 2).transform.GetChild(index % 2).gameObject;
            _wrongButtons[i - 1] = g.GetComponent<Button>();
            TextMeshProUGUI wrongText = g.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            wrongText.text = settings.wrongOptions[i - 1];
            g.SetActive(true);
        }

        for (int i = wrongSize + 1; i < 4; ++i)
        {
            _layout.transform.GetChild(i / 2).transform.GetChild(i % 2).gameObject.SetActive(false);
        }

        anim.gameObject.GetComponent<VerticalLayoutGroup>().spacing = _offsetQuestionButtons;

        _rightButton.onClick.RemoveAllListeners();
        _rightButton.onClick.AddListener(RightOptionChoosed);
        anim = _rightButton.GetComponent<Animator>();
        anim.Rebind();
        anim.Update(0f);
        anim.SetBool("Correct", false);
        anim.SetBool("Wrong", false);

        for (int j = 0; j < wrongSize; ++j)
        {
            _wrongButtons[j].onClick.RemoveAllListeners();
            _wrongButtons[j].onClick.AddListener(WrongOptionChoosed);
            anim = _wrongButtons[j].GetComponent<Animator>();
            anim.Rebind();
            anim.Update(0f);
            anim.SetBool("Correct", false);
            anim.SetBool("Wrong", false);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }

    void RightOptionChoosed()
    {
        if (!_buttonPressed)
        {
            _buttonPressed = true;
            _rightButton.GetComponent<Animator>().SetBool("Correct", true);
            _settings.onCorrectOption.Invoke();
        }
    }

    void WrongOptionChoosed()
    {
        if(!_buttonPressed)
        {
            _buttonPressed = true;
            foreach (Button button in _wrongButtons)
            {
                button.GetComponent<Animator>().SetBool("Wrong", true);
            }
            _settings.onWrongOption.Invoke();
        }
    }

    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }
}
