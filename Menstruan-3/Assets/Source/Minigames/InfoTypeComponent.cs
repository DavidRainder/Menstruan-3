using UnityEngine;

public enum InfoType { NAME, DESCRIPTION};

public class InfoTypeComponent : MonoBehaviour
{
    [SerializeField]
    private InfoType _type;

    [SerializeField]
    private int _index;


    public int GetIndex() {  return _index; }

    public InfoType GetInfoType() {  return _type; }
}
