using System.Collections.Generic;
using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "InitializingWeapons_", menuName = "Configs/Initializing Weapons")]
    public class InitializingWeapons : ScriptableObject
    {
        [SerializeField] private List<WeaponConfig> _weaponConfigs;
        public List<WeaponConfig> WeaponConfigs => _weaponConfigs;
    }
}
