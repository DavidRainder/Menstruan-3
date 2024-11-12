using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [SerializeField]
    private float _offsetQuestDistance;
    [SerializeField]
    private float _offsetButtonDistance;
    [SerializeField]
    private float _offsetTextDistance;
    [SerializeField] 
    private GameObject _questionObject;
    [SerializeField]
    private GameObject _questionObjectText;
    [SerializeField]
    private VerticalLayoutGroup _answers;

    // Only call once
    void AdaptRectsToText()
    {
        TextMeshProUGUI textGUI = _questionObjectText.GetComponent<TextMeshProUGUI>();
        textGUI.rectTransform.offsetMin = new Vector2(_offsetTextDistance, _offsetTextDistance);
        textGUI.rectTransform.offsetMax = new Vector2(-_offsetTextDistance, -_offsetTextDistance);
        _answers.spacing = _offsetButtonDistance;
        RectTransform sizeRect = _questionObject.GetComponent<RectTransform>();
        Rect rect = textGUI.GetPixelAdjustedRect();
        sizeRect.sizeDelta = new Vector2(rect.width + 2 * _offsetTextDistance, rect.height / 1.5f);
        sizeRect.anchoredPosition = new Vector3(0, 0, 0);
        RectTransform answerRect = _answers.GetComponent<RectTransform>();
        RectTransform buttonRect = _answers.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        answerRect.anchoredPosition = new Vector3(sizeRect.sizeDelta.x / 2 - buttonRect.sizeDelta.x / 2, 
            -sizeRect.sizeDelta.y / 2 - _offsetQuestDistance - answerRect.sizeDelta.y / 2);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AdaptRectsToText();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
