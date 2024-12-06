using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    #region Singleton

    private static FadeManager instance = null;
    public static FadeManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else { Destroy(this.gameObject); }
    }

    #endregion
 
    [SerializeField] private Image _fade;
    [SerializeField] private float _timeToFade;

    private float _timer;

    public void FadeIn()
    {
        instance.StartCoroutine(Fade(1.0f, 0.0f, instance._timeToFade));
    }

    public void FadeOut()
    {
        instance.StartCoroutine(Fade(0.0f, 1.0f, instance._timeToFade));
    }

    public void Clear()
    {
        instance.StartCoroutine(Fade(0.0f, 0.0f, 0.1f));
    }

    private IEnumerator Fade(float start, float end, float time)
    {
        instance._fade.color = new Color(instance._fade.color.r, instance._fade.color.g, instance._fade.color.b, start);
        instance._timer = 0.0f;
        while(instance._fade.color.a != end)
        {
            Debug.Log(instance._timer);
            instance._fade.color = new Color(instance._fade.color.r, instance._fade.color.g, instance._fade.color.b, Mathf.Lerp(start, end, instance._timer / time));

            yield return new WaitForNextFrameUnit();
            instance._timer += Time.deltaTime;
        }
    }
}
