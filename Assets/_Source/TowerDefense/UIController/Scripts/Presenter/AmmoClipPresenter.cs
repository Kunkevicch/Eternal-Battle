using System;
using Zenject;

namespace EndlessRoad
{
    public sealed class AmmoClipPresenter : IInitializable, IDisposable
    {
        private readonly WeaponHolder _weaponHolder;
        private readonly AmmoView _ammoView;

        public AmmoClipPresenter(WeaponHolder weaponHolder, AmmoView ammoView)
        {
            _weaponHolder = weaponHolder;
            _ammoView = ammoView;
        }

        public void Initialize()
        {
            _weaponHolder.CurrentAmmoInformer.AmmoInClipChanged += OnAmmoClipChanged;
            //_ammoView.AmmoChange(_weaponHolder.CurrentAmmoInformer.CurrentAmmoInClip.ToString());
        }

        public void Dispose()
        {
            _weaponHolder.CurrentAmmoInformer.AmmoInClipChanged -= OnAmmoClipChanged;
        }

        private void OnAmmoClipChanged(string ammoClip) => _ammoView.AmmoChange(ammoClip);

    }
}
