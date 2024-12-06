using System.Collections;
using UnityEngine;

public class MinigameInstanceManager : MonoBehaviour
{
    #region Singleton

    private static MinigameInstanceManager instance = null;
    public static MinigameInstanceManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    private GameObject _minigameInstance;

    [SerializeField]
    private Transform _positionInstance;

    [SerializeField]
    private Animator _minigameInstanceAnimations;

    [SerializeField]
    private Animator _screenOnAnimation;

    [SerializeField]
    private float _instanceTime = 2.0f;

    public void StartMinigame(GameObject prefab)
    {
        if (instance._minigameInstance!= null)
        {
            Destroy(instance._minigameInstance);
        }

        _minigameInstanceAnimations.enabled = true;
        _minigameInstanceAnimations.SetBool("Instance", true);
        StartCoroutine(InstantiateMinigame(prefab));
    }

    public void StartMinigameNoAnimation(GameObject prefab)
    {
        if (instance._minigameInstance != null)
        {
            Destroy(instance._minigameInstance);
        }

        StartCoroutine(WaitToStart(prefab));
    }


    private void InstantiatePrefab(GameObject prefab)
    {
        _minigameInstance = GameObject.Instantiate(prefab, _positionInstance);
    }

    IEnumerator InstantiateMinigame(GameObject prefab)
    {
        while (IsPlayingAnimation("InstanceMinigame"))
        {
            yield return new WaitForEndOfFrame();
        }

        _screenOnAnimation.enabled = true;
        _screenOnAnimation.Rebind();
        _screenOnAnimation.Update(0f);
        _screenOnAnimation.SetBool("End", false);
        while (_screenOnAnimation.GetCurrentAnimatorStateInfo(0).IsName("ScreenOn"))
        {
            yield return new WaitForEndOfFrame();
        }

        StartMinigameNoAnimation(prefab);
    }


    IEnumerator WaitToStart(GameObject prefab)
    {
        yield return new WaitForSeconds(_instanceTime);
        InstantiatePrefab(prefab);
    }

    public void EndMinigame()
    {
        Destroy(instance._minigameInstance);
        StartCoroutine(StopScreenAnimation());
    }

    public bool IsPlayingAnimation(string animation)
    {
        string name;
        if(animation == "InstanceMinigame")
        {
            name = "DefaultScreen";
        }
        else
        {
            name = "DefaultDialogues";
        }
        return !_minigameInstanceAnimations.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    IEnumerator StopScreenAnimation()
    {
        _screenOnAnimation.SetBool("End", true);
        while (!_screenOnAnimation.GetCurrentAnimatorStateInfo(0).IsName("ScreenGrey"))
        {
            yield return new WaitForEndOfFrame();
        }

        _screenOnAnimation.enabled = false;
        _screenOnAnimation.Rebind();
        _screenOnAnimation.Update(0f);
        _screenOnAnimation.SetBool("End", false);
        _minigameInstanceAnimations.SetBool("Instance", false);
        _minigameInstanceAnimations.SetBool("End", true);
    }

    public void ResetAnim()
    {
        _minigameInstanceAnimations.enabled = false;
        _minigameInstanceAnimations.SetBool("End", false);
    }
}
