using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private WeaponConfig weaponConfig;
        private WeaponView weaponView;

        private ImpactService _impactService;
        private ObjectPool _objectPool;

        [Inject]
        public void Construct(ImpactService impactService, ObjectPool objectPool)
        {
            _impactService = impactService;
            _objectPool = objectPool;
        }

        private void Awake()
        {
            weaponView = weaponConfig.Spawn(transform, this, _objectPool, _impactService);
        }
        // Update is called once per frame
        void Update()
        {
            weaponView.Shoot();
            transform.position += transform.right * Time.deltaTime * 1f;
        }
    }
}
