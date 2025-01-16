using TMPro.EditorUtilities;
using UnityEngine;

public class MuteButton : MonoBehaviour
{
    private Animator _animator;
    public void onMuteClick(bool enabled)
    {
        _animator.SetBool("Mute", enabled);
    }

    void Start()
    {
        GameManager.Instance.RegisterMuteButton(this);
        _animator = GetComponent<Animator>();
        _animator.SetBool("Mute", GameManager.Instance.IsMute());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
