using System;
using System.Collections.Generic;
using Game.Gameplay.Roots;
using UnityEngine;

namespace Game.Gameplay.Islands
{
    public class Island : MonoBehaviour
    {
        public event Action<Island> OnLeftArea;
        public event Action<Island> OnAttached;
        public event Action<Island> OnDetached;

        [SerializeField]
        private float maximumAngularSpeed = 2f;

        public Rigidbody Rigidbody { get; private set; }

        private RootLocation[] _rootLocations = Array.Empty<RootLocation>();

        private readonly List<Island> _attachedIslands = new ();

        public bool IsAttached => _attachedIslands.Count > 0;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _rootLocations = GetComponentsInChildren<RootLocation>();
        }

        private void FixedUpdate ()
        {
            if (Rigidbody.angularVelocity.sqrMagnitude > maximumAngularSpeed * maximumAngularSpeed)
                Rigidbody.angularVelocity = Rigidbody.angularVelocity.normalized * maximumAngularSpeed;
        }

        private void OnEnable ()
        {
            for (int i = 0; i < _rootLocations.Length; i++) {
                _rootLocations[i].OnAttached += OnRootAttached;
                _rootLocations[i].OnDetached += OnRootDetached;
            }
        }

        private void OnDisable ()
        {
            for (int i = 0; i < _rootLocations.Length; i++) {
                _rootLocations[i].OnAttached -= OnRootAttached;
                _rootLocations[i].OnDetached -= OnRootDetached;
            }
        }

        private void OnTriggerExit (Collider other)
        {
            if (other.gameObject.name == "HazardsArea")
                OnLeftArea?.Invoke(this);
        }

        public bool IsAttachedTo (Island island)
        {
            return _attachedIslands.Contains(island);
        }

        private void OnRootAttached (Root root)
        {
            if (IsAttachedTo(root.Island))
                return;

            _attachedIslands.Add(root.Island);
            if (_attachedIslands.Count == 1)
                OnAttached?.Invoke(root.Island);
        }

        private void OnRootDetached (Root root)
        {
            if (!IsAttachedTo(root.Island))
                return;

            _attachedIslands.Remove(root.Island);
            if (_attachedIslands.Count == 0)
                OnDetached?.Invoke(root.Island);
        }
    }
}
