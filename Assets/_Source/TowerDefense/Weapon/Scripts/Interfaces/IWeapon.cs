using System;

namespace EndlessRoad
{
    internal interface IWeapon
    {
        public int CurrentAmmo { get; set; }
        public int CurrentAmmoClip { get; set; }
        public float FireRate { get; }
        public void Fire();
        public void Reload();

        public event Action WeaponReloaded;
    }
}