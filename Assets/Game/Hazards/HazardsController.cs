using System.Collections;
using UnityEngine;

namespace Game.Hazards
{
    public class HazardsController : MonoBehaviour
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
            StartCoroutine(SpawnHazard());
        }

        private IEnumerator SpawnHazard ()
        {
            while (true) {
                int i = Random.Range(0, _hazardControllers.Length);
                _hazardControllers[i].SpawnHazard(transform);

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }
}
