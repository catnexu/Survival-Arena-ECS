using System;
using System.Reflection;
using UnityEditor;

namespace Infrastructure.Editor
{
    public static class SerializedPropertyExtensions
    {
        public static object GetValue(this SerializedProperty property)
        {
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return fi.GetValue(property.serializedObject.targetObject);
        }

        public static void SetValue(this SerializedProperty property, object value)
        {
            Type parentType = property.serializedObject.targetObject.GetType();
            FieldInfo fi = parentType.GetField(property.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(property.serializedObject.targetObject, value);
        }
    }
}