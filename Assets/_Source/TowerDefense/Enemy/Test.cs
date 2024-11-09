using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private WeaponConfig weaponConfig;
        private WeaponView weaponView;

        private ObjectPool _objectPool;

        [Inject]
        public void Construct(ImpactService impactService, ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        private void Awake()
        {
            weaponView = weaponConfig.Spawn(transform, _objectPool);
        }
        // Update is called once per frame
        void Update()
        {
            transform.position += transform.right * Time.deltaTime * 1f;
            weaponView.Tick(true);
        }
    }
}
