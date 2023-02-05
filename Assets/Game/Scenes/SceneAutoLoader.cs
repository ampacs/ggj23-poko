using UnityEngine;
using Zenject;

namespace Game.Scenes
{
    public class SceneAutoLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneContainer scene;
        
        [Inject]
        private ScenesController _scenesController;

        private void Start ()
        {
            _scenesController.LoadScene(scene).Forget();
        }
    }
}
