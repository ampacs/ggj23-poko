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

        [SerializeField]
        private bool isPlayerIsland;

        public Rigidbody Rigidbody { get; private set; }

        [SerializeField]
        private GameObject[] treesSources;
        private Trees.Tree[] _trees;

        private RootLocation[] _rootLocations = Array.Empty<RootLocation>();

        private readonly List<Island> _attachedIslands = new ();

        public bool IsAttached => _attachedIslands.Count > 0;

        public bool IsPlayerIsland => isPlayerIsland;

        public bool IsConnectedToPlayerIsland { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _rootLocations = GetComponentsInChildren<RootLocation>();
            List<Trees.Tree> trees = new(treesSources.Length);
            foreach (GameObject treesSource in treesSources) {
                Trees.Tree[] childTrees = treesSource.GetComponentsInChildren<Trees.Tree>();
                trees.AddRange(childTrees);
            }
            _trees = trees.ToArray();
        }

        private void Start ()
        {
            if (isPlayerIsland) {
                IsConnectedToPlayerIsland = true;
                SetTreesScoringState(true);
            }
        }

        private void OnDestroy ()
        {
            foreach (RootLocation rootLocation in _rootLocations) {
                if (rootLocation.Root != null && rootLocation.Root.Attached)
                    rootLocation.Root.Detach();
                rootLocation.enabled = false;
            }
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
        
        private bool CheckIsConnectedToPlayerIsland ()
        {
            if (IsPlayerIsland) {
                IsConnectedToPlayerIsland = true;
                SetTreesScoringState(true);
                return true;
            }

            List<Island> checkedIslands = new(_attachedIslands.Count);
            List<Island> islandsToCheck = new(_attachedIslands);
            while (islandsToCheck.Count > 0) {
                Island island = islandsToCheck[0];
                islandsToCheck.RemoveAt(0);
                if (island.IsPlayerIsland) {
                    IsConnectedToPlayerIsland = true;
                    SetTreesScoringState(true);
                    return true;
                }
                checkedIslands.Add(island);
                for (int i = 0; i < island._attachedIslands.Count; i++) {
                    Island attachedIsland = island._attachedIslands[i];
                    if (!checkedIslands.Contains(attachedIsland) && !islandsToCheck.Contains(attachedIsland))
                        islandsToCheck.Add(attachedIsland);
                }
            }

            IsConnectedToPlayerIsland = false;
            SetTreesScoringState(false);

            return false;
        }

        private void SetTreesScoringState (bool scoring)
        {
            for (int i = 0; i < _trees.Length; i++)
                _trees[i].Scoring = scoring;
        }

        private void OnRootAttached (Root root)
        {
            if (IsAttachedTo(root.Island))
                return;

            _attachedIslands.Add(root.Island);
            if (_attachedIslands.Count == 1)
                OnAttached?.Invoke(root.Island);

            if (!IsConnectedToPlayerIsland)
                CheckIsConnectedToPlayerIsland();
        }

        private void OnRootDetached (Root root)
        {
            if (!IsAttachedTo(root.Island))
                return;

            _attachedIslands.Remove(root.Island);
            if (_attachedIslands.Count == 0)
                OnDetached?.Invoke(root.Island);

            if (IsConnectedToPlayerIsland)
                CheckIsConnectedToPlayerIsland();
        }
    }
}
