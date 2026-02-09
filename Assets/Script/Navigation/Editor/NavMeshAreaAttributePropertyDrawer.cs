using UnityEditor;
using UnityEngine;

namespace Navigation.Editor
{
    [CustomPropertyDrawer(typeof(NavMeshAreaAttribute))]
    internal sealed  class NavMeshAreaAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            NavMeshComponentsGUIUtility.AreaPopup(position, label.text, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 20;
    }
}