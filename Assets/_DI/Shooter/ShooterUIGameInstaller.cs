using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class ShooterUIGameInstaller : MonoInstaller
    {
        [SerializeField] private TextView _ammoInClipView;
        [SerializeField] private TextView _currentAmmoView;
        [SerializeField] private TextView _waveCountView;
        [SerializeField] private TextView _rewardView;

        [SerializeField] private CrossHairView _crossHairView;

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
        }
    }
}