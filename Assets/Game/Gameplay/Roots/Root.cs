using System;
using Game.Gameplay.Islands;
using UnityEngine;

namespace Game.Gameplay.Roots
{
    public class Root : MonoBehaviour
    {
        public event Action<Root> OnAttached;
        public event Action<Root> OnDetached;
        
        [SerializeField]
        private AnimationCurve attachmentForceOverTime = AnimationCurve.Linear(0, 1, 60, 10);

        [SerializeField]
        private AnimationCurve attachmentDistanceOverTime = AnimationCurve.Linear(0, 10, 60, 2);

        [SerializeField]
        private float attachmentDistanceBreakThreshold = 1f;

        private Root _attachedRoot;

        private Collider _collider;
        public Rigidbody Rigidbody { get; private set; }

        public Island Island { get; private set; }

        public bool Attached { get; private set; }
        public float AttachmentStartMoment { get; private set; }

        public float AttachmentForce => attachmentForceOverTime.Evaluate(Time.fixedTime - AttachmentStartMoment);
        public float AttachmentDistance => attachmentDistanceOverTime.Evaluate(Time.fixedTime - AttachmentStartMoment);

        private void Awake ()
        {
            Rigidbody = transform.parent.GetComponentInParent<Rigidbody>();
            _collider = GetComponent<Collider>();
            Island = GetComponentInParent<Island>();
        }

        private void FixedUpdate ()
        {
            if (!Attached) {
                _collider.enabled = true;
                return;
            }

            if (_attachedRoot == null) {
                Attached = false;
                return;
            }

            Vector3 displacement = transform.position - _attachedRoot.transform.position;
            float attachmentDistance = AttachmentDistance;
            if (displacement.sqrMagnitude > attachmentDistance * attachmentDistance) {
                float attachmentForce = AttachmentForce;
                Vector3 forceDirection = displacement.normalized;

                _attachedRoot.Rigidbody.AddForceAtPosition(forceDirection * attachmentForce, _attachedRoot.transform.position, ForceMode.Force);
            }

            if (displacement.sqrMagnitude > attachmentDistanceBreakThreshold * attachmentDistanceBreakThreshold)
                Detach();
        }

        private void OnCollisionEnter (Collision collision)
        {
            if (Attached)
                return;

            bool success = collision.gameObject.TryGetComponent(out Root otherRoot);
            if (!success || otherRoot.Attached || Island.IsAttachedTo(otherRoot.Island))
                return;
            // get distance between roots
            Debug.Log((transform.position - otherRoot.transform.position).magnitude);
            
            Attached = true;
            _attachedRoot = otherRoot;
            AttachmentStartMoment = Time.fixedTime;
            _collider.enabled = false;
            otherRoot.Attach(this);
            
            OnAttached?.Invoke(this);
        }

        public void Attach (Root otherRoot) => Attach(otherRoot, Time.fixedTime);

        public void Attach (Root otherRoot, float attachmentStartMoment)
        {
            if (Island.IsAttachedTo(otherRoot.Island))
                return;

            Attached = true;
            _attachedRoot = otherRoot;
            AttachmentStartMoment = attachmentStartMoment;
            _collider.enabled = false;

            OnAttached?.Invoke(this);
        }

        public Root Detach ()
        {
            Root otherRoot = _attachedRoot;
            _attachedRoot = null;
            _collider.enabled = true;
            Attached = false;

            otherRoot.DetachInternal();
            
            OnDetached?.Invoke(this);

            return otherRoot;
        }

        private void DetachInternal ()
        {
            _attachedRoot = null;
            _collider.enabled = true;
            Attached = false;

            OnDetached?.Invoke(this);
        }
    }
}
