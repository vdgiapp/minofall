
namespace Minofall.UI
{
    public class GameOverOverlay : UIOverlay
    {
        public void ToggleGameOver(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}