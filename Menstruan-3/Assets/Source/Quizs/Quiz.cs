using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
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
    bool _correctOption;

    float _time;
    [SerializeField]
    private float _afterQuizTime;

    private FMOD.Studio.EventInstance _quizMusic;

    public void SetInfo(QuizSettings settings)
    {
        _time = 0;
        _correctOption = false;

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

        _quizMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Quiz");
        _quizMusic.start();
    }

    void RightOptionChoosed()
    {
        if (!_buttonPressed)
        {
            _buttonPressed = true;
            _rightButton.GetComponent<Animator>().SetBool("Correct", true);
            _quizMusic.setParameterByNameWithLabel("Answer", "Correct answer");
            _quizMusic.setParameterByName("FadeOutTimeline", 2.0f);
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
            _quizMusic.setParameterByNameWithLabel("Answer", "Wrong answer");
            _quizMusic.setParameterByName("FadeOutTimeline", 2.0f);
        }
    }

    private void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetComponent<RectTransform>());
    }

    private void Update()
    {
        if (_buttonPressed)
        {
            _time += Time.deltaTime; // Se podría cambiar a subrutina
            if(_time >= _afterQuizTime)
            {
                _quizMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                if (_correctOption)
                {
                    _settings.onCorrectOption.Invoke();
                }
                else
                {
                    _settings.onWrongOption.Invoke();
                }
                _time = 0;
                _buttonPressed = false;
            }
        }   
    }
}
