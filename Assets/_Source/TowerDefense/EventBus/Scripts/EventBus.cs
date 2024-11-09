using System;

namespace EndlessRoad
{
    public class EventBus
    {
        public event Action<string, string> AmmoChanged;

        public void RaiseAmmoChanged(string ammoInClip, string ammoCurrent) => AmmoChanged?.Invoke(ammoInClip, ammoCurrent);
    }
}
