using UnityEngine;

namespace Game.Hazards.WindStorm
{
    public class HazardWindStormController : MonoBehaviour, IHazardController
    {
        [SerializeField]
        private HazardWindStorm template;

        [SerializeField]
        private float spawnDistance = 15f;

        public void SpawnHazard (Transform target)
        {
            Vector2 spawnPosition = Random.insideUnitCircle * spawnDistance;
            Vector3 spawnPosition3D = new(spawnPosition.x, 0f, spawnPosition.y);
            HazardWindStorm hazard = Instantiate(template, spawnPosition3D, Quaternion.identity, transform);
            hazard.OnCompleted += OnHazardCompleted;
            hazard.Spawn(target.position);
        }

        private void OnHazardCompleted (HazardWindStorm hazard)
        {
            Destroy(hazard.gameObject);
        }
    }
}
