using UnityEngine;

namespace Game.Hazards.Islands
{
    public class HazardIslandController : MonoBehaviour, IHazardController
    {
        [SerializeField]
        private float displacementMultiplier = 1f;

        [SerializeField]
        private float spawnDistanceMultiplier = 1f;

        [SerializeField]
        private HazardIsland hazardIslandTemplate;

        public void SpawnHazard (Transform target)
        {
            Vector2 displacement = Random.insideUnitCircle * displacementMultiplier;
            Vector3 targetPosition = target.position
                                     + new Vector3(displacement.x, 0f, displacement.y);
            displacement = Random.insideUnitCircle * spawnDistanceMultiplier;
            Vector3 spawnPosition = target.position
                                     + new Vector3(displacement.x, 0f, displacement.y);

            HazardIsland island = Instantiate(hazardIslandTemplate, spawnPosition, Quaternion.identity, transform);
            island.OnCompleted += OnHazardCompleted;
            island.Spawn(targetPosition);
        }

        private void OnHazardCompleted (HazardIsland hazard)
        {
            Destroy(hazard.gameObject);
        }
    }
}
