using UnityEngine;

namespace EndlessRoad
{
    public interface IWeaponFactory
    {
        WeaponView InitializeSpawnedWeapon(WeaponConfig weaponData,WeaponView spawnedWeapon);
        WeaponView SpawnWeapon(WeaponConfig weaponData,Transform parent);
    }
}
