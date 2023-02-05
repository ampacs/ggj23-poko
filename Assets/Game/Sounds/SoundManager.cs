using System;
using UnityEngine;
using Zenject;
using System.Collections; // ①


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
          PlayingSound.volume = 0.2f;
          StartCoroutine(Dead());
        }

        
        public void Play_EndingSound()
        {
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

        IEnumerator Dead() {
            {
               yield return new WaitForSeconds( 3.0f );
            }
        }
    }
}
