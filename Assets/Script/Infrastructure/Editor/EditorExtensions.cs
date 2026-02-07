using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.Editor
{
    public static class EditorExtensions
    {
        public static IEnumerable<T> GetAllInstances<T>(params string[] folders) where T : Object => GetAllInstances<T>($"t:{typeof(T)}", folders);

        public static IEnumerable<T> GetAllInstances<T>(string searchStr, params string[] folders) where T : Object =>
            AssetDatabase.FindAssets(searchStr, folders)
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>);
    }
}