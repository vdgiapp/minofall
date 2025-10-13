using System.Collections.Generic;
using UnityEngine;

namespace Minofall
{
    public class AudioLibrary
    {
        private readonly Dictionary<string, AudioClip> _musicClips = new();
        private readonly Dictionary<string, AudioClip> _sfxClips = new();

        public void Initialize()
        {
            LoadClips("Audio/Music", _musicClips);
            LoadClips("Audio/SFX", _sfxClips);
        }

        public AudioClip GetMusicClip(string name) => _musicClips.TryGetValue(name, out var clip) ? clip : null;
        public AudioClip GetSFXClip(string name) => _sfxClips.TryGetValue(name, out var clip) ? clip : null;

        private void LoadClips(string path, Dictionary<string, AudioClip> dict)
        {
            AudioClip[] clips = Resources.LoadAll<AudioClip>(path);
            foreach (AudioClip clip in clips)
            {
                if (!dict.ContainsKey(clip.name))
                {
                    dict[clip.name] = clip;
                }
            }
        }
    }
}