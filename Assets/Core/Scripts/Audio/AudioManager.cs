using UnityEngine;

namespace Minofall
{
    // Singleton in Bootstrapper prefab
    // No need to DontDestroyOnLoad
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource _musicSource1;
        [SerializeField] private AudioSource _musicSource2;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _uiSource;

        private AudioSource _activeMusicSource;

        private void Awake()
        {
            InstanceInit();
            if (_musicSource1 == null) _musicSource1 = transform.Find("MusicSource1").GetComponent<AudioSource>();
            if (_musicSource2 == null) _musicSource2 = transform.Find("MusicSource2").GetComponent<AudioSource>();
            if (_sfxSource == null) _sfxSource = transform.Find("SFXSource").GetComponent<AudioSource>();
            if (_uiSource == null) _uiSource = transform.Find("UISource").GetComponent<AudioSource>();
        }

        private void InstanceInit()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}