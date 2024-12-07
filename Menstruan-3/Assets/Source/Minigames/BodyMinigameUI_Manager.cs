using TMPro;
using UnityEngine;

public class BodyMinigameUI_Manager : MonoBehaviour
{
    [SerializeField]
    GameObject infoGameObject;

    [SerializeField]
    GameObject _correctTextGO;
    TextMeshProUGUI _correctText;

    [SerializeField]
    GameObject _incorrectTextGO;
    TextMeshProUGUI _incorrectText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _correctText = _correctTextGO.GetComponent<TextMeshProUGUI>();
        _incorrectText = _incorrectTextGO.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Aceptar()
    {
        if (infoGameObject != null)
        {
            infoGameObject.SetActive(false);
        }
    }

    public void Comprobar()
    {
        if (infoGameObject != null)
        {
            infoGameObject.SetActive(true);
        }
    }

    public void SetIncorrects(int i)
    {
        _incorrectText.text = "Mal: " + i;
    }

    public void SetCorrects(int i)
    {
        _correctText.text = "Bien: " + i;
    }
}
