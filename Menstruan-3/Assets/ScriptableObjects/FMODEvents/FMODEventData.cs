using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="FMODEventData", menuName="Scriptable Objects/FMODEventData")]
public class FMODEventData : ScriptableObject
{
    public enum PARAM_TYPE
    {
        LABELED,
        CONTINOUOS,
        DISCRETE
    }

    [Serializable]
    public struct FMODEventParameterData
    {
        public PARAM_TYPE type;
        public string name;
        public string value;
    }

    public string eventName;
    public List<FMODEventParameterData> parameters;
}
