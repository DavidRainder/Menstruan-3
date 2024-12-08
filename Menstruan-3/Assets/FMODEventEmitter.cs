using FMOD.Studio;
using System.Collections.Generic;
using UnityEngine;
using static FMODEventData;

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

    private void EmitEvent(FMOD.Studio.EventInstance ev, string eventName)
    {
        ev.start();
        if (instance.activeEvents.ContainsKey(eventName))
        {
            instance.activeEvents[eventName].Add(ev);
        }
        else
        {
            List<FMOD.Studio.EventInstance> events = new List<FMOD.Studio.EventInstance>();
            events.Add(ev);
            instance.activeEvents.Add(eventName, events);
        }
    }

    private FMOD.Studio.EventInstance createEventByName(string name)
    {
        return FMODUnity.RuntimeManager.CreateInstance(
            "event:/" + name);
    }

    public void EmitEvent(string name)
    {
        EmitEvent(createEventByName(name), name);
    }

    public void EmitEvent(FMODEventData ev)
    {
        FMOD.Studio.EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(
            "event:/"+ev.eventName);
        foreach(FMODEventParameterData a in ev.parameters)
        {
            if(a.type == FMODUnity.ParameterType.Labeled)
            {
                eventInstance.setParameterByNameWithLabel(a.name, a.value);
            }
            else
            {
                float aFloat = float.Parse(a.value);
                eventInstance.setParameterByName(a.name, aFloat);
            }
        }

        EmitEvent(eventInstance, ev.eventName);
    }

    public void UpdateEventInstances(FMODEventData ev) {
        if (!instance.activeEvents.ContainsKey(ev.eventName)) return;

        List<FMOD.Studio.EventInstance> eventInstance = instance.activeEvents[ev.eventName];

        foreach (var a in ev.parameters)
        {
            if (a.type == FMODUnity.ParameterType.Labeled)
            {
                foreach(var b in eventInstance)
                {
                    b.setParameterByNameWithLabel(a.name, a.value);
                }
            }
            else
            {
                foreach (var b in eventInstance)
                {
                    float aFloat = float.Parse(a.value);
                    b.setParameterByName(a.name, aFloat);
                }
            }
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

    private void Update()
    {
        List<EventInstance> events = new List<EventInstance>();
        foreach(var a in activeEvents)
        {
            for(int i = 0; i < a.Value.Count; ++i)
            {
                a.Value[i].getPlaybackState(out PLAYBACK_STATE state);
                if(state == PLAYBACK_STATE.STOPPED)
                {
                    events.Add(a.Value[i]);
                }
            }

            foreach(var remove in events)
            {
                a.Value.Remove(remove);
            }

            events.Clear();
        }
    }
}
