using System.Collections;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class ShooterEntryPoint : MonoBehaviour
    {
        //TODO: REMOVE THIS FIELD AFTER ADDING META-MAP WITH LEVEL CHOICE
        [SerializeField] private LevelConfig _levelConfig;

        private InitializingWeapons _weapons;
        private WeaponHolder _weaponHolder;
        private ObjectPool _objectPool;
        private GameObserver _levelObserver;
        private WaveCountPresenter _waveCountPresenter;

        [Inject]
        public void Construct(
            InitializingWeapons weapons
            , ObjectPool objectPool
            , WeaponHolder weaponHolder
            , EnemiesController enemiesController
            , EventBus eventBus
            , GameObserver levelObserver
            , WaveCountPresenter waveCountPresenter
            )
        {
            _weapons = weapons;
            _objectPool = objectPool;
            _weaponHolder = weaponHolder;
            _levelObserver = levelObserver;
            _waveCountPresenter = waveCountPresenter;
        }

        private IEnumerator Start()
        {
            foreach (var weapon in _weapons.WeaponConfigs)
            {
                weapon.SetObjectPool(_objectPool);
                yield return null;
            }
            yield return null;
            _weaponHolder.InitializeWeapons();
            _levelObserver.InitializeLevel(_levelConfig.levelDifficult, _levelConfig.WaveCount);
            _levelObserver.StartLevel();
            _waveCountPresenter.SetWaveCount(_levelConfig.WaveCount);
        }
    }
}
