using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scenes.UI
{
    [RequireComponent(typeof(Button))]
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField]
        private SceneContainer scene;

        [Inject]
        private ScenesController _scenesController;

        private Button _button;

        private void Awake ()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable ()
        {
            _button.onClick.AddListener(LoadScene);
        }

        private void OnDisable ()
        {
            _button.onClick.RemoveListener(LoadScene);
        }

        private void LoadScene ()
        {
            if (_scenesController == null)
                _scenesController = FindObjectOfType<ScenesController>();
            _scenesController.LoadScene(scene).Forget();
        }
    }
}
