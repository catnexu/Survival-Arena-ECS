using UnityEngine;

namespace Unit
{
    [CreateAssetMenu(menuName = "Unit/" + nameof(DefaultConfig), fileName = nameof(DefaultConfig))]
    internal sealed class DefaultConfig : UnitConfig
    {
        public override UnitType Type => UnitType.Default;
    }
}