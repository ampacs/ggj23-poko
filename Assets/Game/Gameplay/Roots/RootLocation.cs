using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Roots
{
    public class RootLocation : MonoBehaviour
    {
        [SerializeField, Min(0)]
        private float growthInterval = 10f;

        [SerializeField]
        private List<Root> rootStages;

        private Root _currentRoot;

        private void Start ()
        {
            Grow(rootStages[0]);
        }

        private void Grow (Root nextRootStage)
        {
            float attachMoment = 0;
            Root otherRoot = null;
            if (_currentRoot != null) {
                if (_currentRoot.Attached) {
                    attachMoment = _currentRoot.AttachmentStartMoment;
                    otherRoot = _currentRoot.Detach();
                }

                Destroy(_currentRoot.gameObject);
            }

            _currentRoot = Instantiate(nextRootStage, transform, false);

            if (otherRoot != null)
                _currentRoot.Attach(otherRoot, attachMoment);
        }
    }
}
