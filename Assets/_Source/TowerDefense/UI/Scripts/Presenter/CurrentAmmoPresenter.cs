using System;
using Zenject;

namespace EndlessRoad
{
    public class CurrentAmmoPresenter : IInitializable, IDisposable
    {
        private readonly WeaponHolder _weaponHolder;
        private readonly TextView _ammoView;

        public CurrentAmmoPresenter(WeaponHolder weaponHolder, TextView ammoView)
        {
            _weaponHolder = weaponHolder;
            _ammoView = ammoView;
        }

        public void Initialize()
        {
            //_ammoView.AmmoChange(_weaponHolder.CurrentAmmoInformer.CurrentAmmo.ToString());
            _weaponHolder.CurrentAmmoInformer.CurrentAmmoChanged += OnCurrentAmmoChanged;
        }

        public void Dispose()
        {
            _weaponHolder.CurrentAmmoInformer.CurrentAmmoChanged -= OnCurrentAmmoChanged;
        }

        private void OnCurrentAmmoChanged(string currentAmmo) => _ammoView.TextChange(currentAmmo);
    }
}
