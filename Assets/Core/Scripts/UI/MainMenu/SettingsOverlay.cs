using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Minofall.Data;

namespace Minofall.UI
{
    public class SettingsOverlay : UIOverlay
    {
        [SerializeField] private TMP_Dropdown controlTypeDropdown;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        protected override void Awake()
        {
            controlTypeDropdown.onValueChanged.AddListener(OnControlTypeChanged);
            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        }

        private void OnDestroy()
        {
            controlTypeDropdown.onValueChanged.RemoveListener(OnControlTypeChanged);
            masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        }

        public void ToggleSettings(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetControlTypeDropdownValue(int value)
        {
            controlTypeDropdown.value = value;
        }

        public void SetMasterVolumeSliderValue(float value)
        {
            masterVolumeSlider.value = value;
            AudioManager.Instance.SetMasterVolume(value / 100f);
        }

        public void SetMusicVolumeSliderValue(float value)
        {
            musicVolumeSlider.value = value;
            AudioManager.Instance.SetMusicVolume(value / 100f);
        }

        public void SetSFXVolumeSliderValue(float value)
        {
            sfxVolumeSlider.value = value;
            AudioManager.Instance.SetSFXVolume(value / 100f);
        }

        private void OnControlTypeChanged(int value)
        {
            PlayerData.Instance.Settings.controlType = value;
        }

        private void OnMasterVolumeChanged(float value)
        {
            float val = value / 100f;
            PlayerData.Instance.Settings.masterVolume = val;
            AudioManager.Instance.SetMasterVolume(val);
        }

        private void OnMusicVolumeChanged(float value)
        {
            float val = value / 100f;
            PlayerData.Instance.Settings.musicVolume = val;
            AudioManager.Instance.SetMusicVolume(value / 100f);
        }

        private void OnSFXVolumeChanged(float value)
        {
            float val = value / 100f;
            PlayerData.Instance.Settings.sfxVolume = val;
            AudioManager.Instance.SetSFXVolume(value / 100f);
        }
    }
}