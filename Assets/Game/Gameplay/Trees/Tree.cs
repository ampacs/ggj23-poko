using System;
using System.Collections.Generic;
using Game.Score;
using UnityEngine;
using Zenject;

namespace Game.Gameplay.Trees
{
    public class Tree : MonoBehaviour
    {
        [SerializeField]
        private List<Configuration> configurations;

        [Inject]
        private ScoreSystem _scoreSystem;

        private int _currentConfigurationIndex = 0;
        private float _currentConfigurationChangeMoment = 0;
        
        public bool Scoring { get; set; }

        private void Awake ()
        {
            if (_scoreSystem == null)
                _scoreSystem = FindObjectOfType<ScoreSystem>();

            for (int i = 1; i < configurations.Count; i++)
                if (configurations[i].model != null)
                    configurations[i].model.SetActive(false);

            _currentConfigurationChangeMoment = Time.time + UnityEngine.Random.
                Range(configurations[0].minDuration, configurations[0].maxDuration);
            if (configurations[0].model != null)
                configurations[0].model.SetActive(true);
        }

        private void FixedUpdate ()
        {
            if (Scoring)
                _scoreSystem.AddScore(Time.fixedDeltaTime * configurations[_currentConfigurationIndex].scoreRate);

            if (Time.time > _currentConfigurationChangeMoment)
                NextConfiguration();
        }

        private void NextConfiguration ()
        {
            if (_currentConfigurationIndex == configurations.Count - 1)
                return;

            _currentConfigurationIndex++;
            _currentConfigurationChangeMoment = Time.time + UnityEngine.Random.
                Range(configurations[_currentConfigurationIndex].minDuration, configurations[_currentConfigurationIndex].maxDuration);

            if (configurations[_currentConfigurationIndex - 1].model != null)
                configurations[_currentConfigurationIndex - 1].model.SetActive(false);
            if (configurations[_currentConfigurationIndex].model != null)
                configurations[_currentConfigurationIndex].model.SetActive(true);
        }

        [Serializable]
        private struct Configuration
        {
            public float scoreRate;
            public float minDuration;
            public float maxDuration;
            public GameObject model;
        }
    }
}
