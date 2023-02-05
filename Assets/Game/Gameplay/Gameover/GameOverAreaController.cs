using Game.Gameplay.Islands;
using Game.Score;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Gameover
{
    public class GameOverAreaController : MonoBehaviour
    {
        // [SerializeField]
        // public AudioSource PlayingSound; 
        // public AudioSource EndingSound; 
        // public AudioSource DeadSound; 
        // public AudioSource WindStormSound; 
        // public AudioSource MergingSound; 

        [SerializeField]
        private GameOverPanel gameOverPanel;

        [Inject]
        private ScoreSystem _scoreSystem;

        //[Inject]
        public SoundManager _soundManager;

        private void OnTriggerEnter (Collider other)
        {
            if (!other.TryGetComponent(out Island island) || !island.IsPlayerIsland)
                return;

            _soundManager.Play_DeadSound();
            gameOverPanel.Show();
            Destroy(island.gameObject);

            _scoreSystem.CommitScore();
        }
    }
}
