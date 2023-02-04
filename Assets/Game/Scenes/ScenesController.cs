using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Game.Scenes
{
    public class ScenesController : MonoInstaller
    {
        public delegate UniTask LoadAction(Scene scene, CancellationToken token);
        public delegate UniTask ProgressAction (Scene scene, float progress, CancellationToken token);

        [SerializeField]
        private SceneContainer startingScene;

        private bool _isSceneLoading;
        private Scene _current;

        private readonly List<LoadAction> _onSceneLoadStartActions = new();
        private readonly List<ProgressAction> _onSceneLoadUpdateActions = new();
        private readonly List<LoadAction> _onSceneLoadFinishActions = new();

        public override void InstallBindings ()
        {
            Container.Bind<ScenesController>().FromInstance(this).AsSingle();
        }

        public void AddOnSceneLoadStartAction (LoadAction action)
        {
            _onSceneLoadStartActions.Add(action);
        }

        public void AddOnSceneLoadUpdateAction (ProgressAction action)
        {
            _onSceneLoadUpdateActions.Add(action);
        }

        public void AddOnSceneLoadFinishAction (LoadAction action)
        {
            _onSceneLoadFinishActions.Add(action);
        }

        public void RemoveOnSceneLoadStartAction (LoadAction action)
        {
            _onSceneLoadStartActions.Remove(action);
        }

        public void RemoveOnSceneLoadUpdateAction (ProgressAction action)
        {
            _onSceneLoadUpdateActions.Remove(action);
        }

        public void RemoveOnSceneLoadFinishAction (LoadAction action)
        {
            _onSceneLoadFinishActions.Remove(action);
        }

        public async UniTaskVoid LoadScene (Scene scene)
        {
            await LoadSceneInternal(scene, CancellationToken.None);
        }

        public async UniTaskVoid LoadScene (Scene scene, CancellationToken token)
        {
            await LoadSceneInternal(scene, token);
        }

        private async UniTask LoadSceneInternal (Scene scene, CancellationToken token)
        {
            if (_isSceneLoading)
                // TODO: throw exception or return error
                return;

            // combine the tokens
            CancellationTokenSource combinedCancellationTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(token, this.GetCancellationTokenOnDestroy());
            token = combinedCancellationTokenSource.Token;

            _isSceneLoading = true;
            _current = scene;

            foreach (LoadAction action in _onSceneLoadStartActions) {
                await action(scene, token);

                if (token.IsCancellationRequested) {
                    Debug.LogError("Canceled loading of scene");
                    return;
                }
            }

            AsyncOperation operation = SceneManager.LoadSceneAsync(scene.MainScene.BuildIndex, LoadSceneMode.Single);
            operation.allowSceneActivation = false;

            float previousProgress;
            do {
                previousProgress = operation.progress;

                foreach (ProgressAction action in _onSceneLoadUpdateActions) {
                    await action(scene, operation.progress, token);

                    if (token.IsCancellationRequested) {
                        Debug.LogError("Canceled loading of scene");
                        return;
                    }
                }

                await UniTask.NextFrame(cancellationToken: token);
                if (token.IsCancellationRequested) {
                    Debug.LogError("Canceled loading of scene");
                    return;
                }
            } while (!Mathf.Approximately(previousProgress, operation.progress));

            foreach (ProgressAction action in _onSceneLoadUpdateActions) {
                await action(scene, 1, token);

                if (token.IsCancellationRequested) {
                    Debug.LogError("Canceled loading of scene");
                    return;
                }
            }

            await UniTask.NextFrame(cancellationToken: token);

            operation.allowSceneActivation = true;

            await UniTask.WaitUntil(() => operation.isDone, cancellationToken: token);

            foreach (LoadAction action in _onSceneLoadFinishActions) {
                await action(scene, token);

                if (token.IsCancellationRequested) {
                    Debug.LogError("Canceled loading of scene");
                    return;
                }
            }

            _isSceneLoading = false;
        }
    }
}
