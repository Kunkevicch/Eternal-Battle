using System.Reflection;

namespace EndlessRoad
{
    public class ModifireDamage : ModifireBase<int>
    {
        public override void Apply(WeaponView weapon)
        {
            AttributeName = "_stats/_damage";
            int damage = GetAttribute<int>(weapon,out object targetObject, out FieldInfo field);

            damage += Amount;
            field.SetValue(targetObject,damage);
        }
    }
}
