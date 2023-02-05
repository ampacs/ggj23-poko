using System;
using UnityEngine;
using Game.Gameplay;

namespace Game.Hazards.WindStorm
{
    [RequireComponent(typeof(Rigidbody))]
    public class HazardWindStorm : MonoBehaviour, IHazard
    {
        public event Action<HazardWindStorm> OnCompleted;

        [SerializeField]
        private float travelSpeed;

        [SerializeField]
        private float travelDuration;

        private float _spawnMoment;
        private Vector3 _movementDirection;

        private Transform _transform;
        private Rigidbody _rigidbody;
        

        public GameObject _soundManager;
        private bool Playing;
        

        public void Spawn (Vector3 targetPosition)
        {
            _movementDirection = (targetPosition - _transform.position).normalized;
            _transform.rotation = Quaternion.LookRotation(_movementDirection);
            _spawnMoment = Time.fixedTime;

            enabled = true;
        }

        private void Awake ()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
           
            enabled = false;
        }

        private void Start()
        {
            _soundManager = GameObject.Find("BGM");
            
        }

        private void FixedUpdate ()
        {
            _rigidbody.MovePosition(transform.position + _movementDirection * (travelSpeed * Time.fixedDeltaTime));

            if (Time.fixedTime - _spawnMoment > travelDuration) {
                OnCompleted?.Invoke(this);
                enabled = false;
            }

            // if(Mathf.Abs(_transform.position - gameObject.transform.position) < 1f)
            if(Vector3.Distance(_transform.position, gameObject.transform.position)<1f && !Playing)
            {
                _soundManager.GetComponent<SoundManager>().Play_WindSound();
                Playing = true;
            }
            
        }

        // TODO: add handling of colliding with roots and trees
    }
}
