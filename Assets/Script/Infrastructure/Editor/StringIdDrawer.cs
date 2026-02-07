using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.Editor
{
    public abstract class StringIdDrawer : PropertyDrawer
    {
        private const float MinWindowWidth = 300f;

        private StringListSearchProvider _window;
        private Object _object;
        private bool _getObjectTried;
        protected virtual bool DrawField => false;
        protected virtual Type ObjectType => null;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (DrawField)
            {
                height *= 2;
            }

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, property.displayName, "ERROR: May only apply to type string");
                return;
            }

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                Rect buttonRect = EditorGUI.PrefixLabel(position, label);
                buttonRect.height = EditorGUIUtility.singleLineHeight;
                if (GUI.Button(buttonRect, property.stringValue, EditorStyles.popup))
                {
                    _window ??= ScriptableObject.CreateInstance<StringListSearchProvider>();
                    _window.Construct(GetIds(property), s => Callback(property, s));
                    float windowWidth = Mathf.Max(MinWindowWidth, buttonRect.width);
                    SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition),
                        requestedWidth: windowWidth), _window);
                }
            }

            if (DrawField)
            {
                if (!_getObjectTried)
                {
                    _getObjectTried = true;
                    _object = GetObject(property.stringValue);
                }

                using (new EditorGUI.DisabledScope(true))
                {
                    position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    position.height = EditorGUIUtility.singleLineHeight;
                    EditorGUI.ObjectField(position, _object, ObjectType, _object);
                }
            }
        }

        private void Callback(SerializedProperty property, string value)
        {
            property.stringValue = value;
            property.serializedObject.ApplyModifiedProperties();
            _getObjectTried = false;
        }

        protected abstract List<string> GetIds(SerializedProperty property);

        protected virtual Object GetObject(string value)
        {
            return null;
        }
    }
}