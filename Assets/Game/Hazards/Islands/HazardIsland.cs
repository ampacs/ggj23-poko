using System;
using Game.Gameplay.Islands;
using UnityEngine;

namespace Game.Hazards.Islands
{
    public class HazardIsland : MonoBehaviour
    {
        public event Action<HazardIsland> OnCompleted;

        [SerializeField]
        private float travelSpeed;

        [SerializeField]
        private float rotationSpeed;

        [SerializeField]
        private Island island;

        private Vector3 _movementDirection;

        private Transform _transform;

        public void Spawn (Vector3 targetPosition)
        {
            _movementDirection = (targetPosition - _transform.position).normalized;
            _transform.rotation = Quaternion.LookRotation(_movementDirection);

            island.OnLeftArea += SignalCompleted;

            enabled = true;
        }

        private void Awake ()
        {
            _transform = transform;

            enabled = false;
        }

        private void OnEnable ()
        {
            island.OnAttached += Disable;
        }

        private void OnDisable ()
        {
            island.OnAttached -= Disable;
        }

        private void OnDestroy ()
        {
            Destroy(island);
        }

        private void Disable (Island other)
        {
            enabled = false;
        }

        private void FixedUpdate ()
        {
            island.Rigidbody.AddForce(_movementDirection * travelSpeed, ForceMode.Force);
            island.Rigidbody.angularVelocity = Vector3.up * rotationSpeed;
            if (island.Rigidbody.velocity.sqrMagnitude > travelSpeed * travelSpeed) {
                island.Rigidbody.velocity = _movementDirection * travelSpeed;
            }
        }

        private void SignalCompleted (Island island)
        {
            OnCompleted?.Invoke(this);
            enabled = false;
        }
    }
}
