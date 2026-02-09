using Infrastructure.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Weapon.Editor
{
    [CustomPropertyDrawer(typeof(WeaponIdAttribute))]
    internal sealed class WeaponIdAttributeDrawer : StringIdDrawer
    {
        protected override bool DrawField => true;
        protected override Type ObjectType => typeof(WeaponConfig);
        
        protected override List<string> GetIds(SerializedProperty property) =>
            EditorExtensions.GetAllInstances<WeaponConfig>().Select(w => w.name).ToList();
        
        protected override Object GetObject(string value)
        {
            return EditorExtensions.GetAllInstances<WeaponConfig>().FirstOrDefault(a => a.name.Equals(value));
        }
    }
}