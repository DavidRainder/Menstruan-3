using UnityEngine;

enum InfoType { NAME, DESCRIPTION};

public class InfoTypeComponent : MonoBehaviour
{
    [SerializeField]
    private InfoType _type;

    [SerializeField]
    private int _index;
}
