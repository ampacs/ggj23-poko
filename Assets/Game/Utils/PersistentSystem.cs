using UnityEngine;

namespace Game.Utils
{
    public class PersistentSystem : MonoBehaviour
    {
        private static PersistentSystem _instance;

        private void Awake()
        {
            if (_instance == null) {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            } else if (_instance != this)
                Destroy(gameObject);
        }
    }
}
