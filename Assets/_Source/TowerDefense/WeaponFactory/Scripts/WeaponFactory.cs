using UnityEngine;

namespace EndlessRoad
{
    public class WeaponFactory : IWeaponFactory
    {
        private readonly ObjectPool _objectPool;

        public WeaponFactory(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        public WeaponView InitializeSpawnedWeapon(
            WeaponConfig weaponData
            , WeaponView spawnedWeapon
            )
        {
            ShootRaycastStrategyBase shootStrategy = GetShootStrategyByWeaponType(weaponData);
            spawnedWeapon.Initialize(
                weaponData.ShootConfiguration.ImpactMask
                , weaponData
                , shootStrategy
                );

            return spawnedWeapon;
        }

        public WeaponView SpawnWeapon(WeaponConfig weaponData, Transform parent)
        {
            WeaponView weaponView = GameObject.Instantiate(weaponData.WeaponViewPrefab.gameObject).GetComponent<WeaponView>();
            weaponView.transform.SetParent(parent, false);

            ShootRaycastStrategyBase shootStrategy =
                GetShootStrategyByWeaponType(weaponData);

            weaponView.Initialize(
                weaponData.ShootConfiguration.ImpactMask
                , weaponData
                , shootStrategy
                );

            return weaponView;
        }

        private ShootRaycastStrategyBase GetShootStrategyByWeaponType(WeaponConfig weaponData)
        {
            return weaponData.Type switch
            {
                AttackType.SingleRaycast 
                => new ShootRaycastStrategySingle(_objectPool, weaponData.TrailConfiguration.shootTrail),
                
                AttackType.MultipleRaycast 
                => new ShootRaycastStrategyMultiple(_objectPool, weaponData.TrailConfiguration.shootTrail, weaponData.ProjectileCount),
                
                _ => new ShootRaycastStrategySingle(_objectPool, weaponData.TrailConfiguration.shootTrail),
            };
        }
    }
}
