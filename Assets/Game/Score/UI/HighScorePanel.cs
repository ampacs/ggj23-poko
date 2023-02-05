using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Score.UI
{
    public class HighScorePanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        [Inject]
        private ScoreSystem _scoreSystem;

        private void Reset ()
        {
            scoreText = GetComponent<TMP_Text>();
        }

        private void Start ()
        {
            UpdateScore(_scoreSystem.HighScore);
        }

        private void UpdateScore (float score)
        {
            scoreText.text = score.ToString("F0");
        }
    }
}
