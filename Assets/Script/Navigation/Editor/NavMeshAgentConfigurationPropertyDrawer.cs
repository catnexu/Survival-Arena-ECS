using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Navigation.Editor
{
    [CustomPropertyDrawer(typeof(NavMeshAgentConfiguration))]
    internal sealed class NavMeshAgentConfigurationPropertyDrawer : PropertyDrawer
    {
        private float _additiveHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                EditorGUI.PropertyField(position, property, label, true);
                _additiveHeight = 0f;
                if (property.isExpanded)
                {
                    _additiveHeight = EditorGUIUtility.singleLineHeight;
                    Rect buttonRect = AlignBottom(position, EditorGUIUtility.singleLineHeight);
                    buttonRect = AlignCenterX(buttonRect, buttonRect.width *= 0.5f);
                    if (GUI.Button(buttonRect, "Copy settings by Id"))
                    {
                        NavMeshAgentConfiguration value = (NavMeshAgentConfiguration) property.boxedValue;
                        value = CreateFromType(value);
                        property.boxedValue = value;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) + _additiveHeight;
        }

        private static NavMeshAgentConfiguration CreateFromType(NavMeshAgentConfiguration current)
        {
            NavMeshBuildSettings settings = NavMesh.GetSettingsByID(current.AgentTypeId);
            return current.ModifyBySettings(settings);
        }

        private static Rect AlignBottom(Rect rect, float height)
        {
            rect.y = rect.y + rect.height - height;
            rect.height = height;
            return rect;
        }

        private static Rect AlignCenterX(Rect rect, float width)
        {
            rect.x = (float) ((double) rect.x + (double) rect.width * 0.5 - (double) width * 0.5);
            rect.width = width;
            return rect;
        }
    }
}