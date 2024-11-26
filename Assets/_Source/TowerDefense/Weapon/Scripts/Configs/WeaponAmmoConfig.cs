using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "AmmoConfig_", menuName = "Configs/Weapon/Ammo Config", order = 4)]
    public class WeaponAmmoConfig : ScriptableObject
    {
        public int MaxAmmo;
        public int ClipSize;
        public int CurrentAmmo;
        public int CurrentClipAmmo;
    }
}
