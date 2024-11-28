using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
public class AnimationManager : MonoBehaviour
{
    #region Singleton

    private static AnimationManager instance = null;
    public static AnimationManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField] private GameObject animationPrefab;
    [SerializeField] private Transform animationPosition;

    private GameObject _animationInstance;

    public void StartAnimation(string animation)
    {
        if (instance._animationInstance != null)
        {
            Destroy(instance._animationInstance);
        }

        instance._animationInstance = Instantiate(animationPrefab);
        instance._animationInstance.transform.position = animationPosition.position;
        Animator animator = instance._animationInstance.GetComponent<Animator>();
        animator.SetBool("Appear", true);
        animator = animator.transform.GetChild(0).GetComponent<Animator>();
        animator.SetBool(animation, true);
    }

    public void EndAnimation()
    {
        Animator animator = instance._animationInstance.GetComponent<Animator>();
        animator.SetBool("Disappear", true);
        animator.SetBool("Appear", false);
    }
}
