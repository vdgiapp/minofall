using UnityEngine;
using UnityEngine.Audio;

namespace Minofall
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance
        { get; private set; }

        [Header("Mixer Settings")]
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private string _masterVolumeParam = "MasterVolume";
        [SerializeField] private string _musicVolumeParam = "MusicVolume";
        [SerializeField] private string _sfxVolumeParam = "SFXVolume";
        [SerializeField] private string _uiVolumeParam = "UIVolume";

        [Header("AudioSource Settings")]
        [SerializeField] private AudioSource _musicSource1;
        [SerializeField] private AudioSource _musicSource2;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _uiSource;

        private AudioSource _activeMusicSource;
        private AudioSource _inactiveMusicSource;

        private AudioLibrary _audioLibrary;
        private VolumeData _volumeData;

        private void Awake()
        {
            // Singleton init
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Set default audio library
            _audioLibrary = new AudioLibrary();

            // Set default volume data
            _volumeData = new VolumeData();

            // Set default active/inactive music sources
            _activeMusicSource = _musicSource1;
            _inactiveMusicSource = _musicSource2;
        }

        public void PlaySFX(string soundName)
        {
            var clip = _audioLibrary.GetSFXClip(soundName);
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip);
        }

        public void PlayUISound(string soundName)
        {
            var clip = _audioLibrary.GetUIClip(soundName);
            if (clip == null) return;
            _uiSource.PlayOneShot(clip);
        }
    }
}