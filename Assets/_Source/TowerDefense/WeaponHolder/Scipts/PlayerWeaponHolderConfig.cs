using System.Collections.Generic;
using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "PlayerWeaponHolderConfig", menuName = "Configs/Weapon/Player Weapon Holder")]
    public class PlayerWeaponHolderConfig : ScriptableObject
    {
        [field: SerializeField] public List<WeaponConfig> _weapons { get; private set; }
    }
}
