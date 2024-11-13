using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    DialogSettings startingDialog;

    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Talk()
    {
        _animator.SetBool("isTalking", true);
        startingDialog.onFinishDialog.AddListener(StopTalking);
        DialogManager.Instance.StartDialog(startingDialog);
    }

    public void StopTalking()
    {
        _animator.SetBool("isTalking", false);
    }
}