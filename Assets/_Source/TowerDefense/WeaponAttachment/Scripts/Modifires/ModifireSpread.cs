using System.Reflection;
using UnityEngine;

namespace EndlessRoad
{
    public class ModifireSpread : ModifireBase<Vector3>
    {
        public override void Apply(WeaponView weapon)
        {
            AttributeName = "_stats/_spread";
            Vector3 oldSpread = GetAttribute<Vector3>(weapon, out object targetObject, out FieldInfo field);

            oldSpread = new(oldSpread.x + Amount.x, oldSpread.y + Amount.y, oldSpread.z + Amount.z);

            field.SetValue(targetObject, oldSpread);
        }
    }
}
