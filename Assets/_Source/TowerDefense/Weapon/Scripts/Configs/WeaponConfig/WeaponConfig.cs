using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "Weapon_", menuName = "Configs/Weapon/Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public AttackType Type;
        public WeaponView WeaponViewPrefab;
        public float ReloadTime;
        public string Name;
        public int Damage;
        public int ProjectileCount;

        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;
        public Vector3 AimPoint;
        public Vector3 AimRotation;

        public AnimatorOverrideController AnimatorController;

        public WeaponShootConfiguration ShootConfiguration;
        public WeaponTrailConfiguration TrailConfiguration;
        public WeaponAudioConfig WeaponAudioConfig;
        public WeaponAmmoConfig WeaponAmmoConfig;

        private ObjectPool _objectPool;

        public void SetObjectPool(ObjectPool objectPool)
        {
            if (_objectPool != null)
                return;

            _objectPool = objectPool;
        }

        public WeaponView Spawn(Transform parent)
        {
            WeaponView weaponView = Instantiate(WeaponViewPrefab.gameObject).GetComponent<WeaponView>();
            weaponView.transform.SetParent(parent, false);
            ShootRaycastStrategyBase shootStrategy = GetShootStrategyByWeaponType();
            weaponView.Initialize(
                ShootConfiguration.ImpactMask
                , this
                , shootStrategy
                );

            return weaponView;
        }

        public WeaponView InitializeWeapon(WeaponView weapon)
        {
            ShootRaycastStrategyBase shootStrategy = GetShootStrategyByWeaponType();
            weapon.Initialize(
                ShootConfiguration.ImpactMask
                , this
                , shootStrategy
                );

            return weapon;
        }

        private ShootRaycastStrategyBase GetShootStrategyByWeaponType()
        {
            switch (Type)
            {
                case AttackType.SingleRaycast:
                    return new ShootRaycastStrategySingle(_objectPool, TrailConfiguration.shootTrail);

                case AttackType.MultipleRaycast:
                    return new ShootRaycastStrategyMultiple(_objectPool, TrailConfiguration.shootTrail, ProjectileCount);

                default: return new ShootRaycastStrategySingle(_objectPool, TrailConfiguration.shootTrail);
            }
        }
    }
}
