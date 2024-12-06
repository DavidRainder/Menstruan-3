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

        _minigameInstance = GameObject.Instantiate(prefab, _positionInstance);
    }


    IEnumerator InstantiateMinigame(GameObject prefab)
    {
        while (IsPlayingAnimation("InstanceMinigame"))
        {
            yield return new WaitForEndOfFrame();
        }

        StartMinigameNoAnimation(prefab);
    }

    public void EndMinigame()
    {
        Destroy(instance._minigameInstance);
        _minigameInstanceAnimations.SetBool("Instance", false);
        _minigameInstanceAnimations.SetBool("End", true);
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

    public void ResetAnim()
    {
        _minigameInstanceAnimations.enabled = false;
        _minigameInstanceAnimations.SetBool("End", false);
    }
}
