using Cysharp.Threading.Tasks;
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

        [Header("AudioSource Settings")]
        [SerializeField] private AudioSource _musicSource1;
        [SerializeField] private AudioSource _musicSource2;
        [SerializeField] private AudioSource _sfxSource;

        private readonly AudioLibrary _audioLibrary = new();
        private AudioSource _activeMusicSource;
        private AudioSource _inactiveMusicSource;

        private string _currentTrack;
        private bool _isCrossfading;

        private void Awake()
        {
            // Singleton init
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Init audio library
            _audioLibrary.Initialize();

            // Set default active/inactive music sources
            _activeMusicSource = _musicSource1;
            _inactiveMusicSource = _musicSource2;
        }

        public async void PlayMusic(string trackName, float fadeDuration = 1.5f, bool loop = true)
        {
            if (_isCrossfading || string.Equals(trackName, _currentTrack))
                return;

            var clip = _audioLibrary.GetMusicClip(trackName);
            if (clip == null)
                return;

            _isCrossfading = true;
            _currentTrack = trackName;

            _inactiveMusicSource.clip = clip;
            _inactiveMusicSource.loop = loop;
            _inactiveMusicSource.volume = 0f;
            _inactiveMusicSource.Play();

            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float progress = Mathf.Clamp01(t / fadeDuration);
                _inactiveMusicSource.volume = progress;
                _activeMusicSource.volume = 1f - progress;
                await UniTask.Yield();
            }

            _activeMusicSource.Stop();
            _inactiveMusicSource.volume = 1f;

            (_activeMusicSource, _inactiveMusicSource) = (_inactiveMusicSource, _activeMusicSource);
            _isCrossfading = false;
        }

        public void StopMusic(float fadeDuration = 1f)
        {
            FadeOutMusic(_activeMusicSource, fadeDuration).Forget();
        }

        private async UniTaskVoid FadeOutMusic(AudioSource source, float duration)
        {
            float startVolume = source.volume;
            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                await UniTask.Yield();
            }

            source.Stop();
            source.volume = 1f;
        }

        public void PlaySFX(string soundName)
        {
            var clip = _audioLibrary.GetSFXClip(soundName);
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip);
        }

        public void SetMasterVolume(float value)
        {
            SetVolume(_masterVolumeParam, value);
        }

        public void SetMusicVolume(float value)
        {
            SetVolume(_musicVolumeParam, value);
        }

        public void SetSFXVolume(float value)
        {
            SetVolume(_sfxVolumeParam, value);
        }

        private void SetVolume(string param, float value)
        {
            value = Mathf.Clamp01(value);
            float db = LinearToDb(value);
            _audioMixer.SetFloat(param, db);
        }

        public float GetMasterVolume()
        {
            return GetVolume(_masterVolumeParam);
        }

        public float GetMusicVolume()
        {
            return GetVolume(_musicVolumeParam);
        }

        public float GetSFXVolume()
        {
            return GetVolume(_sfxVolumeParam);
        }

        public float GetVolume(string param)
        {
            _audioMixer.GetFloat(param, out float db);
            return DbToLinear(db);
        }

        private float LinearToDb(float value)
        {
            // 0 (mute) → -80dB, 1 → 0dB
            return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        }

        private float DbToLinear(float db)
        {
            return Mathf.Pow(10f, db / 20f);
        }
    }
}