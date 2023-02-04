using System;
using Zenject;

namespace Game.Score
{
    public class ScoreSystem : MonoInstaller
    {
        public event Action<float> OnScoreUpdated; 
        public event Action<float> OnHighscoreUpdated; 

        public float Score { get; private set; }

        public float HighScore { get; private set; }

        public override void InstallBindings()
        {
            Container.Bind<ScoreSystem>().FromInstance(this).AsSingle();
        }

        public void AddScore(float score)
        {
            if (score <= 0)
                return;

            Score += score;
            OnScoreUpdated?.Invoke(Score);
        }

        public void CommitScore()
        {
            if (Score > HighScore) {
                HighScore = Score;
                OnHighscoreUpdated?.Invoke(Score);
            }

            Score = 0;
        }
    }
}
