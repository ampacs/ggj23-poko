using UnityEngine;

namespace Game.Gameplay.Gameover
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        private void Start ()
        {
            Hide();
        }

        public void Show ()
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide ()
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
