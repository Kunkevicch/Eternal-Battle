using System.Reflection;
using UnityEngine;

namespace EndlessRoad
{
    public class ModifireReloadTIme : ModifireBase<float>
    {
        public override void Apply(WeaponView weapon)
        {
            AttributeName = "_stats/_reloadTime";
            float reloadTime = GetAttribute<float>(weapon, out object targetObject, out FieldInfo field);
        }
    }
}
