using System;
using UnityEngine;
using Zenject;

namespace EndlessRoad
{
    public class CrossHairPresenter : IInitializable, IDisposable
    {
        private CrossHairView _view;
        private WeaponHolder _weaponHolder;

        public CrossHairPresenter(CrossHairView view, WeaponHolder weaponHolder)
        {
            _view = view;
            _weaponHolder = weaponHolder;
        }

        public void Initialize()
        {
            _weaponHolder.WeaponChanged += OnWeaponChanged;
            _weaponHolder.WeaponShooted += OnWeaponShooted;
        }

        public void Dispose()
        {
            _weaponHolder.WeaponChanged -= OnWeaponChanged;
            _weaponHolder.WeaponShooted -= OnWeaponShooted;
        }

        private void OnWeaponChanged()
        {
        }

        private void OnWeaponShooted(bool isShooted)
        {
            if (isShooted)
            {
                _view.UpdateHeight(_weaponHolder.ActiveWeapon.GetForwardDirection().y * 100f);
                _view.UpdateSize(_weaponHolder.ActiveWeapon.GetCurrentSpread() * 1000);
            }
            else
            {
                _view.StartResetSize();
                _view.StartResetHeight();
            }
        }
    }
}
