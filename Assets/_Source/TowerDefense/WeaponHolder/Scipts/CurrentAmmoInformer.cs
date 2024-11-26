using System;

namespace EndlessRoad
{
    public class CurrentAmmoInformer
    {
        private string _currentAmmoInClip;
        private string _currentAmmo;

        public string CurrentAmmoInClip
        {
            get => _currentAmmoInClip;

            set
            {
                _currentAmmoInClip = value;
                AmmoInClipChanged?.Invoke(value);
            }
        }

        public string CurrentAmmo
        {
            get => _currentAmmo;

            set
            {
                _currentAmmo = value;
                CurrentAmmoChanged?.Invoke(value);
            }
        }

        public event Action<string> CurrentAmmoChanged;
        public event Action<string> AmmoInClipChanged;
    }
}
