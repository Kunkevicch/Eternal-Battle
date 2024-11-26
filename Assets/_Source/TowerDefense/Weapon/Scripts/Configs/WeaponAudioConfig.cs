using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "AudioConfig_", menuName = "Configs/Weapon/Audio Config", order = 3)]
    public class WeaponAudioConfig : ScriptableObject
    {
        [Range(0, 1f)] public float Volume;
        public AudioClip[] FireClips;
        public AudioClip[] EquipClips;
        public AudioClip EmptyClip;
        public AudioClip ReloadClip;
        public AudioClip LastBulletClip;

        public void PlayShootingClip(AudioSource source, bool isLastBullet = false)
        {
            if (isLastBullet)
            {
                source.PlayOneShot(LastBulletClip, Volume);
            }
            else
            {
                source.PlayOneShot(FireClips[Random.Range(0, FireClips.Length)], Volume);
            }
        }

        public void PlayEquipClip(AudioSource source)
        {
            source.PlayOneShot(EquipClips[Random.Range(0, EquipClips.Length)], Volume);
        }

        public void PlayeReloadClip(AudioSource source)
        {
            source.PlayOneShot(ReloadClip, Volume);
        }

        public void PlayEmptyClip(AudioSource source)
        {
            source.PlayOneShot(EmptyClip, Volume);
        }
    }
}
