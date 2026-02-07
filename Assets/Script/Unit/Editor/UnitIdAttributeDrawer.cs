using Infrastructure.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Unit.Editor
{
    [CustomPropertyDrawer(typeof(UnitIdAttribute))]
    internal sealed class UnitIdAttributeDrawer : StringIdDrawer
    {
        protected override bool DrawField => true;
        protected override Type ObjectType => typeof(UnitConfig);
        
        protected override List<string> GetIds(SerializedProperty property) =>
            EditorExtensions.GetAllInstances<UnitConfig>().Select(w => w.name).ToList();
        
        protected override Object GetObject(string value)
        {
            return EditorExtensions.GetAllInstances<UnitConfig>().FirstOrDefault(a => a.name.Equals(value));
        }
    }
}