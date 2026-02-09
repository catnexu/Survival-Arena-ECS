using System.Linq;
using Infrastructure.Editor;
using UnityEditor;
using UnityEngine;

namespace Weapon.Editor
{
    [CustomEditor(typeof(WeaponConfigDatabase), true), CanEditMultipleObjects]
    internal sealed class WeaponConfigDatabaseEditor : UnityEditor.Editor
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
            WeaponConfig[] projectUnits = EditorExtensions.GetAllInstances<WeaponConfig>().ToArray();
            for (int i = 0; i < configs.Length; i++)
            {
                if (configs[i] is WeaponConfigDatabase config)
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