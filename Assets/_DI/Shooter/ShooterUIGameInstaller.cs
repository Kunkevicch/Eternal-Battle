using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class ShooterUIGameInstaller : MonoInstaller
    {
        [SerializeField] private AmmoView _ammoInClipView;
        [SerializeField] private AmmoView _currentAmmoView;
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
        }
    }
}