using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Hazards
{
    public class HazardsManager : MonoBehaviour
    {
        [SerializeField]
        private float spawnInterval = 5f;

        private IHazardController[] _hazardControllers;

        private void Awake ()
        {
            _hazardControllers = GetComponentsInChildren<IHazardController>();
        }

        private void Start ()
        {
            StartCoroutine(SpawnHazards());
        }

        private IEnumerator SpawnHazards ()
        {
            while (true) {
                yield return new WaitForSeconds(spawnInterval);

                int i = Random.Range(0, _hazardControllers.Length);
                // TODO: start using player's island transform instead of HazardManager's
                _hazardControllers[i].SpawnHazard(transform);
            }
        }
    }
}
