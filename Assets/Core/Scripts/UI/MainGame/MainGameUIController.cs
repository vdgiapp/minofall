using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Minofall.UI
{
    public class MainGameUIController : MonoBehaviour
    {
        [Header("Overlays")]
        public IngameOverlay IngameOverlay;
        public PauseMenuOverlay PauseMenuOverlay;
        public CountdownOverlay CountdownOverlay;
        public GameOverOverlay GameOverOverlay;

        [Header("Sprites")]
        [Tooltip("0 = I, 1 = J, 2 = L, 3 = O, 4 = S, 5 = T, 6 = Z")]
        [SerializeField] private Sprite[] _pieceSprites;
        [SerializeField] private Sprite[] _countdownSprites;

        // Bỏ ký tự cuối: [..^1]
        public void UpdateLines(int lines)
        {
            string colorString = ColorUtility.ToHtmlStringRGBA(IngameOverlay.GetLinesTextColor());
            IngameOverlay.UpdateLinesText($"<sprite name=\"line\" color=#{colorString[..^1]}/>{lines}");
        }

        public void UpdateScore(int score)
        {
            string colorString = ColorUtility.ToHtmlStringRGBA(IngameOverlay.GetScoreTextColor());
            IngameOverlay.UpdateScoreText($"<sprite name=\"star\" color=#{colorString[..^1]}/>{score:N0}");
        }

        public void UpdateBestScore(int score)
        {
            string colorString = ColorUtility.ToHtmlStringRGBA(IngameOverlay.GetBestScoreTextColor());
            IngameOverlay.UpdateBestScoreText($"<sprite name=\"crown\" color=#{colorString[..^1]}/>{score:N0}");
        }

        public void UpdateLevel(int level)
        {
            IngameOverlay.UpdateLevelText($"{level:D2}");
        }

        public void UpdateNextPiece(int nextImageIndex, int tetrominoIndex, Color color)
        {
            IngameOverlay.UpdateNextPieceSprite(nextImageIndex, _pieceSprites[tetrominoIndex]);
            IngameOverlay.UpdateNextPieceColor(nextImageIndex, color);
        }

        public void UpdateHoldPiece(int tetrominoIndex, Color color)
        {
            IngameOverlay.UpdateHoldPieceSprite((tetrominoIndex < 0) ? null : _pieceSprites[tetrominoIndex]);
            IngameOverlay.UpdateHoldPieceColor(color);
        }

        public async UniTask CountdownAndWaitAsync(int index, int waitTime = 1000)
        {
            CountdownOverlay.SetCountdownSprite(_countdownSprites[index]);
            await UniTask.Delay(waitTime);
        }
    }
}