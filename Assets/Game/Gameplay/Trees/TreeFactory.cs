using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Trees
{
    public class TreeFactory : MonoBehaviour
    {
        [SerializeField]
        private Tree treeTemplate;

        private readonly List<Transform> _spawnPoints = new();

        private void Awake ()
        {
            foreach (Transform child in transform) {
                _spawnPoints.Add(child);
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(treeTemplate, child.position, rotation, child);
            }
        }
    }
}
