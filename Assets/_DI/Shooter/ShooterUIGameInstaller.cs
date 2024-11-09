using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class ShooterUIGameInstaller : MonoInstaller
    {
        [SerializeField] private AmmoView _ammoInClipView;
        [SerializeField] private AmmoView _currentAmmoView;

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
        }
    }
}