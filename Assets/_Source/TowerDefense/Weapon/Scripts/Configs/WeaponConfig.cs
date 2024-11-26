using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "Gun_", menuName = "Configs/Weapon/Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        public ShootType Type;
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

        public WeaponView Spawn(Transform parent, ObjectPool objectPool)
        {
            WeaponView weaponView = Instantiate(WeaponViewPrefab.gameObject).GetComponent<WeaponView>();
            weaponView.transform.SetParent(parent, false);
            //weaponView.transform.localPosition = SpawnPoint;
            //weaponView.transform.localRotation = Quaternion.Euler(SpawnRotation);
            ShootRaycastStrategyBase shootStrategy = GetShootStrategyByWeaponType(objectPool);
            weaponView.Initialize(
                ShootConfiguration.ImpactMask
                , this
                , shootStrategy
                );

            return weaponView;
        }

        private ShootRaycastStrategyBase GetShootStrategyByWeaponType(ObjectPool objectPool)
        {
            switch (Type)
            {
                case ShootType.SingleRaycast:
                    return new ShootRaycastStrategySingle(objectPool, TrailConfiguration.shootTrail);

                case ShootType.MultipleRaycast:
                    return new ShootRaycastStrategyMultiple(objectPool, TrailConfiguration.shootTrail, ProjectileCount);

                default: return new ShootRaycastStrategySingle(objectPool, TrailConfiguration.shootTrail);
            }
        }
    }
}
