using System;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class SoundManager : MonoBehaviour//MonoInstaller
    {
        [SerializeField]
        private AudioSource SoundPlayer;

        [SerializeField]
        public AudioSource PlayingSound; 
        public AudioSource EndingSound; 
        public AudioSource DeadSound; 
        public AudioSource WindStormSound; 
        public AudioSource MergingSound; 

        // public event Action<float> OnScoreUpdated; 
        // public event Action<float> OnHighscoreUpdated; 

        // public float Score { get; private set; }

        // public float HighScore { get; private set; }

        // public override void InstallBindings()
        // {
        //     Container.Bind<SoundManager>().FromInstance(this).AsSingle().NonLazy();
        // }

        // public override void Start()
        // {

        // }

        public void Play_DeadSound()
        {
          DeadSound.Play();
          PlayingSound.Stop();
          EndingSound.Play();
          EndingSound.loop = true;
        }

        public void Play_WindSound()
        {
            Debug.Log("Wind~~");
            WindStormSound.Play();
        }

        public void Play_MergingSound()
        {
            MergingSound.Play();
        }
    }
}
