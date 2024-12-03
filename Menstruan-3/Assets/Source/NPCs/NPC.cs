using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private static int _id;

    bool moving = false;

    [SerializeField] string ID = "null_id";

    [SerializeField] Animator _animator;

    private void Start()
    {
        _animator ??= GetComponentInChildren<Animator>();
        if(ID == "null_id") { ID = "NPC_" + _id.ToString(); }
        ++_id;
        NPCManager.Instance.RegisterNPC(ID, this);
    }

    public void Talk()
    {
        _animator.SetBool("isTalking", true);
    }

    public void StopTalking()
    {
        _animator.SetBool("isTalking", false);
    }

    public void MoveToPoint(Vector3 position)
    {
        if(!moving) StartCoroutine(_MoveToPoint(position));
    }

    IEnumerator _MoveToPoint(Vector3 position)
    {
        moving = true;
        
        Transform target = transform.GetChild(0);
        Transform body = target.GetChild(1);

        float speed = 1.0f;

        float initialDistance = Vector3.Distance(position, target.position);
        
        int dir;
        if (target.position.x < position.x) dir = 1;
        else dir = -1;

        while ((target.position - position).magnitude > 0.05f)
        {
            float currentDistance = Vector3.Distance(position, target.position);
            target.position = Vector3.Lerp(target.position, position, ( speed / (currentDistance / initialDistance))* Time.deltaTime);
            target.rotation = Quaternion.Lerp(
                Quaternion.identity, 
                Quaternion.Euler(new Vector3(0, 0, -dir*30)), 
                (currentDistance - currentDistance*(currentDistance/initialDistance)) / (initialDistance * .25f));
            body.rotation = Quaternion.Lerp(
                Quaternion.identity,
                Quaternion.Euler(new Vector3(0, 0, -dir * 15)),
                (currentDistance - currentDistance * (currentDistance / initialDistance)) / (initialDistance * .25f))
                * target.rotation;
            yield return null;
        }
        target.position = position;
        moving = false;
    }
}