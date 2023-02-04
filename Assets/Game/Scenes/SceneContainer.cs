using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace Game.Scenes
{
    [CreateAssetMenu(fileName = "Scene", menuName = "Scenes/Scene Container", order = 1)]
    public class SceneContainer : ScriptableObject
    {
        [SerializeField]
        private SceneReference mainSceneReference;

        [SerializeField]
        private List<SceneReference> otherSceneReferences;

        public Scene Scene => new(mainSceneReference, otherSceneReferences);

        public static implicit operator Scene (SceneContainer sceneContainer)
        {
            return sceneContainer.Scene;
        }
    }
}
