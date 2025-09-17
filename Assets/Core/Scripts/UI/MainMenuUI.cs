using System;
using UnityEngine;

namespace Minofall
{
    public class MainMenuUI : MonoBehaviour
    {
        #region [Events] Button click
        public event Action OnPlayButtonClicked;
        public event Action OnNewGameButtonClicked;
        public event Action OnHighScoresButtonClicked;
        public event Action OnTutorialsButtonClicked;
        public event Action OnSettingsButtonClicked;
        public event Action OnAboutButtonClicked;
        #endregion

        #region [Methods] Raise event
        public void PlayButtonClicked() =>
            OnPlayButtonClicked?.Invoke();

        public void NewGameButtonClicked() =>
            OnNewGameButtonClicked?.Invoke();

        public void HighScoresButtonClicked() =>
            OnHighScoresButtonClicked?.Invoke();

        public void TutorialsButtonClicked() =>
            OnTutorialsButtonClicked?.Invoke();

        public void SettingsButtonClicked() =>
            OnSettingsButtonClicked?.Invoke();

        public void AboutButtonClicked() =>
            OnAboutButtonClicked?.Invoke();
        #endregion
    }
}