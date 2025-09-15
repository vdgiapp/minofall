using UnityEngine;
using UnityEngine.Audio;

namespace Minofall
{
    /// <summary>
    /// Là một singleton, quản lý tất cả các hoạt động liên quan đến âm thanh trong trò chơi.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance
        { get; private set; }

        [SerializeField] private AudioMixer _audioMixer;

        [SerializeField] private AudioSource _musicSource1;
        [SerializeField] private AudioSource _musicSource2;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _uiSource;

        private AudioSource _activeMusicSource;

        private void Awake()
        {
            // Singleton init
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}