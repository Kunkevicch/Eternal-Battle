using UnityEngine;

namespace EndlessRoad
{
    [CreateAssetMenu(fileName = "AudioConfig_", menuName = "Configs/Weapon/Audio Config", order = 3)]
    public class WeaponAudioConfig : ScriptableObject
    {
        [Range(0, 1f)] public float MinVolume;
        [Range(0, 1f)] public float MaxVolume;
        public AudioClip[] FireClips;
        public AudioClip[] EquipClips;
        public AudioClip EmptyClip;
        public AudioClip ReloadClip;
        public AudioClip LastBulletClip;

        public void PlayShootingClip(AudioSource source, bool isLastBullet = false)
        {
            if (isLastBullet)
            {
                source.PlayOneShot(LastBulletClip, Random.Range(MinVolume, MaxVolume)) ;
            }
            else
            {
                source.PlayOneShot(FireClips[Random.Range(0, FireClips.Length)], Random.Range(MinVolume, MaxVolume));
            }
        }

        public void PlayEquipClip(AudioSource source)
        {
            source.PlayOneShot(EquipClips[Random.Range(0, EquipClips.Length)], Random.Range(MinVolume, MaxVolume));
        }

        public void PlayeReloadClip(AudioSource source)
        {
            source.PlayOneShot(ReloadClip, Random.Range(MinVolume, MaxVolume));
        }

        public void PlayEmptyClip(AudioSource source)
        {
            source.PlayOneShot(EmptyClip, Random.Range(MinVolume, MaxVolume));
        }
    }
}
