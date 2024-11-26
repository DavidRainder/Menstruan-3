using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    #region Singleton

    private static NPCManager instance = null;
    public static NPCManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _npcs = new Dictionary<string, NPC>();
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    [SerializeField] private Transform[] _positions = null;

    public enum NPCSceneDestinations
    {
        LEFT,
        CENTER_LEFT,
        CENTER,
        CENTER_RIGHT,
        RIGHT,
    }

    Dictionary<string, NPC> _npcs;

    private void Start()
    {
        if(_positions == null)
        {
            _positions = transform.GetChild(0).GetComponentsInChildren<Transform>();
        }
    }

    public void RegisterNPC(string ID, NPC npc)
    {
        if (_npcs.ContainsKey(ID)) { return; }
        else _npcs.Add(ID, npc);
    }

    //public void MoveNPC(NPCMovement movement, string ID)
    //{
    //    if (!_npcs.ContainsKey(ID)) { return; }
    //    if (movement.moveByCoordinates)
    //    {
    //        _npcs[movement.id].MoveToPoint(movement.destinationPoint + movement.displacement);
    //    }
    //    else _npcs[movement.id].MoveToPoint(_positions[(int)movement.destination].position);
    //}

    public void MoveNPC(NPCMovement movement)
    {
        if (!_npcs.ContainsKey(movement.id)) { return; }
        if (movement.moveByCoordinates)
        {
            _npcs[movement.id].MoveToPoint(movement.destinationPoint + movement.displacement);
        }
        else _npcs[movement.id].MoveToPoint(_positions[(int)movement.destination].position);
    }

    public void TalkNPC(DialogSettings settings, string npcID)
    {
        _npcs[npcID].Talk(settings);
    }
}
