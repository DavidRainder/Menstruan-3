using UnityEngine;

public class TEST_ShowNextText : MonoBehaviour
{
    [SerializeField] private DialogSettings _settings;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DialogManager.Instance.StartDialog(_settings);
        }
    }

}
