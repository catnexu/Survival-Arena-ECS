using UnityEditor;
using UnityEngine;

namespace Navigation.Editor
{
    [CustomPropertyDrawer(typeof(NavMeshAgentAttribute))]
    internal sealed class NavMeshAgentAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            NavMeshComponentsGUIUtility.AgentTypePopup(position, label.text, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            NavMeshComponentsGUIUtility.IsAgentSelectionValid(property) ? 20 : 40;
    }
}