using System;
using UnityEditor;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NPCMovement", menuName = "Scriptable Objects/NPCMovement")]
public class NPCMovement : ScriptableObject
{
    public bool moveByCoordinates = false;
    public string id = "";

    /// <summary>
    /// Punto final a donde queremos que el personaje llegue
    /// </summary>
    public Vector3 destinationPoint = Vector3.zero;
    /// <summary>
    /// Lo que queremos movernos. Será aplicado a destination.
    /// Si destination es (0,0,0) solo nos moveremos displacement.
    /// </summary>
    public Vector3 displacement = Vector3.zero;

    public NPCManager.NPCSceneDestinations destination;

#if UNITY_EDITOR
    [CustomEditor(typeof(NPCMovement))]
    public class NPCMovementEditor : Editor
    {
        private SerializedProperty[] _properties;
        private void OnEnable()
        {
            string[] propertyNames = {
                "moveByCoordinates", "id", "destinationPoint", "displacement", "destination"
            };
            _properties = new SerializedProperty[propertyNames.Length];
            for (int i = 0; i < propertyNames.Length; i++)
            {
                _properties[i] = serializedObject.FindProperty(propertyNames[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            DrawProperty(_properties[1], "ID");
            DrawInspectorHeader("Movement Type");
            DrawProperty(_properties[0], "Moved by Coordinates");

            if (_properties[0].boolValue)
            {
                DrawInspectorHeader("Coordinates");
                DrawProperty(_properties[2], "Destination Point");
                DrawProperty(_properties[3], "Displacement");
            }
            else
            {
                DrawInspectorHeader("Relative Position");
                DrawEnumProperty(_properties[4], "Destination", typeof(NPCManager.NPCSceneDestinations));
            }

            EditorGUI.EndChangeCheck();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawInspectorHeader(string text)
        {
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }

        private void DrawEnumProperty(SerializedProperty prop, string label, System.Type enumType)
        {
            if (!enumType.IsEnum)
            {
                Debug.LogError($"{enumType} is not an Enum type.");
                return;
            }
            Enum newEnumValue = EditorGUILayout.EnumPopup(label, (Enum)Enum.ToObject(enumType, prop.enumValueIndex));
            prop.enumValueIndex = Convert.ToInt32(newEnumValue);
        }

        private void DrawProperty(SerializedProperty prop, string label)
        {
            EditorGUILayout.PropertyField(prop, new GUIContent(label));
        }
    }
#endif
}