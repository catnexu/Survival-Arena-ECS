using UnityEditor;
using UnityEngine;

namespace Navigation.Editor
{
    [CustomPropertyDrawer(typeof(NavMeshAreaMaskAttribute))]
    internal sealed class NavMeshAreaMaskAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            NavMeshComponentsGUIUtility.AreaMaskPopup(position, label.text, property);
        }
    }
}