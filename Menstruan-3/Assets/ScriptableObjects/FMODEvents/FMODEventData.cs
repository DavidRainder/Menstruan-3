using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="FMODEventData", menuName="Scriptable Objects/FMODEventData")]
public class FMODEventData : ScriptableObject
{
    [Serializable]
    public struct FMODEventParameterData
    {
        public FMODUnity.ParameterType type;
        public string name;
        public string value;
    }

    public string eventName;
    public List<FMODEventParameterData> parameters;
}
