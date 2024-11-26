using UnityEngine;

namespace EndlessRoad
{
    public struct WeaponAmmo
    {
        private int _maxAmmo;
        private int _clipSize;

        private int _currentAmmo;
        private int _currentClipAmmo;

        public WeaponAmmo(
            int maxAmmo
            , int clipSize
            )
        {
            _maxAmmo = maxAmmo;
            _clipSize = clipSize;

            _currentAmmo = _maxAmmo;
            _currentClipAmmo = _clipSize;
        }

        public int MaxAmmo => _maxAmmo;

        public int ClipSize => _clipSize;

        public int CurrentAmmo => _currentAmmo;

        public int CurrentClipAmmo
        {
            get => _currentClipAmmo;

            private set
            {
                _currentClipAmmo = Mathf.Max(0, value);
            }
        }

        public void DecreaseAmmo() => CurrentClipAmmo--;

        public void Reload()
        {
            int maxReloadAmount = Mathf.Min(_clipSize, _currentAmmo);
            int availableBulletInCurrentClip = _clipSize - _currentClipAmmo;
            int reloadAmmo = Mathf.Min(maxReloadAmount, availableBulletInCurrentClip);

            _currentClipAmmo += reloadAmmo;
            _currentAmmo -= reloadAmmo;
        }

        public bool CanReload() => _currentClipAmmo < _clipSize && _currentAmmo > 0;
        public bool IsEmpty() => _currentClipAmmo == 0;
    }
}
