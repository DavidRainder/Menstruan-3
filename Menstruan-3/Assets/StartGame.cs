using System.Collections;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(_start());
    }

    IEnumerator _start()
    {
        yield return new WaitForSeconds(1.0f);
        StringManager.Instance.ChangeGender(0);
        GameManager.Instance.StartDialog("0_MOM");
        GameManager.StartMusic(0.3f);
    }
}
