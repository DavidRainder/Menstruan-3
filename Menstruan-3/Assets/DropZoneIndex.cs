using UnityEngine;

public class DropZoneIndex : MonoBehaviour
{
    [Tooltip("Índice en el que 'debe estar' para considerarse en una posición correcta un objeto de tipo Drag")]
    [SerializeField]
    private int _indexRequired;

    public int GetIndex() { return _indexRequired; }
}
