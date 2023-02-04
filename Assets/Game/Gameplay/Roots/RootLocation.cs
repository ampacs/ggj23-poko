using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Roots
{
    public class RootLocation : MonoBehaviour
    {
        public event Action<Root> OnAttached;
        public event Action<Root> OnDetached;

        [SerializeField, Min(0)]
        private float growthInterval = 10f;

        [SerializeField]
        private List<Root> rootStages;

        private Root _currentRoot;

        public Root Root => _currentRoot;

        private void Awake ()
        {
            Grow(rootStages[0]);
        }

        private void Grow (Root nextRootStage)
        {
            float attachMoment = 0;
            Root otherRoot = null;
            if (_currentRoot != null) {
                _currentRoot.OnAttached -= SignalAttached;
                _currentRoot.OnDetached -= SignalDetached;

                if (_currentRoot.Attached) {
                    attachMoment = _currentRoot.AttachmentStartMoment;
                    otherRoot = _currentRoot.Detach();
                }

                Destroy(_currentRoot.gameObject);
            }

            _currentRoot = Instantiate(nextRootStage, transform, false);

            if (otherRoot != null)
                _currentRoot.Attach(otherRoot, attachMoment);

            _currentRoot.OnAttached += SignalAttached;
            _currentRoot.OnDetached += SignalDetached;
        }
        
        private void SignalAttached (Root root)
        {
            OnAttached?.Invoke(root);
        }

        private void SignalDetached (Root root)
        {
            OnDetached?.Invoke(root);
        }
    }
}
