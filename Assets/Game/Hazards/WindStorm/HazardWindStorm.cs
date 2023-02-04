using System;
using UnityEngine;

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

        private void FixedUpdate ()
        {
            _rigidbody.MovePosition(transform.position + _movementDirection * (travelSpeed * Time.fixedDeltaTime));

            if (Time.fixedTime - _spawnMoment > travelDuration) {
                OnCompleted?.Invoke(this);
                enabled = false;
            }
        }

        // TODO: add handling of colliding with roots and trees
    }
}
