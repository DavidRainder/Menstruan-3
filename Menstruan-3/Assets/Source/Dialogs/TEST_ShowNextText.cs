using UnityEngine;
using System.Collections.Generic;

public class TEST_ShowNextText : MonoBehaviour
{
    [SerializeField] private List<DialogSettings> _settings;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DialogManager.Instance.StartDialog(_settings[0]);
            Destroy(this.gameObject);
        }
    }
}
