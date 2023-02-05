using Game.Gameplay.Islands;
using Game.Score;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Gameover
{
    public class GameOverAreaController : MonoBehaviour
    {
        [SerializeField]
        private GameOverPanel gameOverPanel;

        [Inject]
        private ScoreSystem _scoreSystem;

        private void OnTriggerEnter (Collider other)
        {
            if (!other.TryGetComponent(out Island island) || !island.IsPlayerIsland)
                return;

            gameOverPanel.Show();
            Destroy(island.gameObject);

            _scoreSystem.CommitScore();
        }
    }
}
