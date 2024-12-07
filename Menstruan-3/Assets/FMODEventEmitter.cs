using System.Collections.Generic;
using UnityEngine;

public class FMODEventEmitter : MonoBehaviour
{
    #region Singleton

    private static FMODEventEmitter instance = null;
    public static FMODEventEmitter Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else { Destroy(this.gameObject); }
    }

    #endregion

    void Start()
    {
        activeEvents = new Dictionary<string, List<FMOD.Studio.EventInstance>>();
    }

    Dictionary<string, List<FMOD.Studio.EventInstance>> activeEvents;

    public void EmitEvent(string name)
    {
        FMOD.Studio.EventInstance ev = FMODUnity.RuntimeManager.CreateInstance(
            "event:/"+name);
        ev.start();
        if(instance.activeEvents.ContainsKey(name))
        {
            instance.activeEvents[name].Add(ev);
        }
        else
        {
            List<FMOD.Studio.EventInstance> events = new List<FMOD.Studio.EventInstance>();
            events.Add(ev);
            instance.activeEvents.Add(name, events);
        }
    }

    public void StopEvents(string name)
    {
        if (!instance.activeEvents.ContainsKey(name)) return;
        
        foreach(var ev in instance.activeEvents[name])
        {
            ev.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    } 
}
