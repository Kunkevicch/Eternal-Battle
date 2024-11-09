using System;
using System.Reflection;

namespace EndlessRoad
{
    public abstract class ModifireBase<T>
    {
        public string AttributeName;
        public T Amount;

        public abstract void Apply(WeaponView weapon);

        protected FieldType GetAttribute<FieldType>(
            WeaponView weapon
            , out object targetObject
            , out FieldInfo field
            )
        {
            string[] paths = AttributeName.Split("/");
            string attribute = paths[paths.Length - 1];

            Type type = weapon.GetType();
            object target = weapon;

            for (int i = 0; i < paths.Length - 1; i++)
            {
                FieldInfo fieldInfo = type.GetField(paths[i], BindingFlags.NonPublic | BindingFlags.Instance);

                if (fieldInfo == null)
                {
                    UnityEngine.Debug.LogError($"Unable to apply modifier" +
                        $" to attribute {AttributeName} because it does not exist on weapon {weapon}");
                }
                else
                {
                    target = fieldInfo.GetValue(target);
                    type = target.GetType();
                }
            }

            FieldInfo attributeField = type.GetField(attribute, BindingFlags.NonPublic | BindingFlags.Instance);

            if (attributeField == null)
            {
                UnityEngine.Debug.LogError($"Unable to apply modifier to attribute " +
                    $"{AttributeName} because it does not exist on weapon {weapon}");
            }

            field = attributeField;
            targetObject = target;
            return (FieldType)attributeField.GetValue(target);
        }
    }
}
