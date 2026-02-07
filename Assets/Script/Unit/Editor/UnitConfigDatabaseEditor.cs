using System.Linq;
using Infrastructure.Editor;
using UnityEditor;
using UnityEngine;

namespace Unit.Editor
{
    [CustomEditor(typeof(UnitConfigDatabase), true), CanEditMultipleObjects]
    internal sealed class UnitConfigDatabaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Initialize"))
            {
                Initialize(targets);
                AssetDatabase.SaveAssets();
            }
        }

        private static void Initialize<T>(params T[] configs) where T : Object
        {
            UnitConfig[] projectUnits = EditorExtensions.GetAllInstances<UnitConfig>().ToArray();
            for (int i = 0; i < configs.Length; i++)
            {
                if (configs[i] is UnitConfigDatabase config)
                {
                    var so = new SerializedObject(config);
                    so.FindProperty("_configs").SetValue(projectUnits);
                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(config);
                }
            }
        }
    }
}