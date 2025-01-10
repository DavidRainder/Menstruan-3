using UnityEngine;

public class DropZoneIndex : MonoBehaviour
{
    [Tooltip("�ndice en el que 'debe estar' para considerarse en una posici�n correcta un objeto de tipo Drag")]
    [SerializeField]
    private int _indexRequired;

    public int GetIndex() { return _indexRequired; }
}
