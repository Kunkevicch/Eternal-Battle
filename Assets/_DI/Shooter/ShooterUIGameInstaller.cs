using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class ShooterUIGameInstaller : MonoInstaller
    {
        [SerializeField] private PlayerConfigShooter _playerConfigShooter;

        [SerializeField] private TextView _ammoInClipView;
        [SerializeField] private TextView _currentAmmoView;
        [SerializeField] private TextView _waveCountView;
        [SerializeField] private TextView _rewardView;

        [SerializeField] private ProgressFlowView _playerHealthProgressView;
        [SerializeField] private TextView _playerHealthTextView;

        [SerializeField] private Health _playerHealth;
        [SerializeField] private CrossHairView _crossHairView;
        [SerializeField] private ProgressFlowView _survivalTimerView;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AmmoClipPresenter>()
                .AsSingle()
                .WithArguments(_ammoInClipView)
                .NonLazy();

            Container.BindInterfacesAndSelfTo<CurrentAmmoPresenter>()
                .AsSingle()
                .WithArguments(_currentAmmoView)
                .NonLazy();

            Container.BindInterfacesAndSelfTo<CrossHairPresenter>()
                .AsSingle()
                .WithArguments(_crossHairView)
                .NonLazy();

            Container.BindInterfacesAndSelfTo<WaveCountPresenter>()
                .AsSingle()
                .WithArguments(_waveCountView)
                .NonLazy();

            Container.BindInterfacesAndSelfTo<RewardObserver>()
               .AsSingle()
               .NonLazy();

            Container.BindInterfacesAndSelfTo<RewardPresenter>()
                .AsSingle()
                .WithArguments(_rewardView)
                .NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerHealthPresenter>()
                .AsSingle()
                .WithArguments(_playerHealth, _playerHealthProgressView, _playerHealthTextView)
                .NonLazy();

            Container.BindInterfacesAndSelfTo<SurvivalTimerPresenter>()
                .AsSingle()
                .WithArguments(_survivalTimerView, _playerConfigShooter)
                .NonLazy();
        }
    }
}