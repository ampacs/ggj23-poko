using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Score.UI
{
    public class ScorePanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        [Inject]
        private ScoreSystem _scoreSystem;

        private void Reset ()
        {
            scoreText = GetComponent<TMP_Text>();
        }

        private void OnEnable ()
        {
            
            _scoreSystem.OnScoreUpdated += UpdateScore;
            // _scoreSystem.OnHighscoreUpdated += UpdateHighscore;
        }
        
        private void OnDisable ()
        {
            _scoreSystem.OnScoreUpdated -= UpdateScore;
            // _scoreSystem.OnHighscoreUpdated -= UpdateHighscore;
        }

        private void UpdateScore (float score)
        {
            scoreText.text = score.ToString("F0");
        }
    }
}
