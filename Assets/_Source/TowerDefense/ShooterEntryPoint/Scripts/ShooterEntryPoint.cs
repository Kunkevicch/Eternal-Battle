using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class ShooterEntryPoint : MonoBehaviour
    {
        //TODO: REMOVE THIS FIELD AFTER ADDING META-MAP WITH LEVEL CHOICE
        [SerializeField] private LevelConfig _levelConfig;

        private WeaponHolder _weaponHolder;
        private GameObserver _levelObserver;
        private WaveCountPresenter _waveCountPresenter;

        [Inject]
        public void Construct(
            WeaponHolder weaponHolder
            , GameObserver levelObserver
            , WaveCountPresenter waveCountPresenter
            )
        {
            _weaponHolder = weaponHolder;
            _levelObserver = levelObserver;
            _waveCountPresenter = waveCountPresenter;
        }

        private void Start()
        {
            _weaponHolder.InitializeWeapons();
            _levelObserver.InitializeLevel(_levelConfig.levelDifficult, _levelConfig.WaveCount);
            _levelObserver.StartLevel();
            _waveCountPresenter.SetWaveCount(_levelConfig.WaveCount);
        }
    }
}
