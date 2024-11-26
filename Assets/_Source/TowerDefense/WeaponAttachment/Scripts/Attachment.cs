using UnityEngine;

namespace EndlessRoad
{
    public class Attachment : MonoBehaviour, IAttachment
    {
        [SerializeField] private bool _isSpread;
        [SerializeField] private Vector3 _spread;
        [SerializeField] private bool _isFireRate;
        [SerializeField] private bool _isDamage;
        [SerializeField] private bool _isDistance;
        [SerializeField] private bool _isAmmo;
        [SerializeField] private bool _isReloadTime;

        public void Apply(WeaponView weaponView)
        {
            ModifireSpread modifire = new();

            modifire.Amount = _spread;
            modifire.Apply(weaponView);
        }
    }
}
