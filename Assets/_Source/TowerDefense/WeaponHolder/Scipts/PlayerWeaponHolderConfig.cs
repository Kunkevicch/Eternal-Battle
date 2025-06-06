using System.Collections.Generic;
using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "PlayerWeaponHolderConfig", menuName = "Configs/Weapon/Player Weapon Holder")]
    public class PlayerWeaponHolderConfig : ScriptableObject
    {
        [SerializeField] private List<WeaponConfig> _weapons;

        public void SpawnWeaponInHolder(
            WeaponHolder weaponHolder
            )
        {
            foreach (var weapon in _weapons)
            {
                WeaponView weaponView = weapon.Spawn(weaponHolder.transform);
                weaponHolder.AddWeapon(weaponView, weapon.AnimatorController);
            }
        }
    }
}
