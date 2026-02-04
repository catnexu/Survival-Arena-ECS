using System;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Infrastructure
{
    internal sealed class UnityEventHolder : IDisposable
    {
        public event Action OnUpdate;
        public event Action OnLateUpdate;
        public event Action OnFixedUpdate;

        private bool _isInjected;

        public UnityEventHolder()
        {
            InjectIntoPlayerLoop();
        }

        public void Dispose()
        {
            RemoveFromPlayerLoop();
        }

        private void InjectIntoPlayerLoop()
        {
            if (_isInjected)
                return;

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            playerLoop = InsertSystem<Update.ScriptRunBehaviourUpdate>(
                playerLoop,
                typeof(UnityEventHolderUpdate),
                () => OnUpdate?.Invoke()
            );

            playerLoop = InsertSystem<FixedUpdate.ScriptRunBehaviourFixedUpdate>(
                playerLoop,
                typeof(UnityEventHolderFixedUpdate),
                () => OnFixedUpdate?.Invoke()
            );

            playerLoop = InsertSystem<PreLateUpdate.ScriptRunBehaviourLateUpdate>(
                playerLoop,
                typeof(UnityEventHolderLateUpdate),
                () => OnLateUpdate?.Invoke()
            );

            PlayerLoop.SetPlayerLoop(playerLoop);
            _isInjected = true;
        }

        private void RemoveFromPlayerLoop()
        {
            if (!_isInjected)
                return;

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            playerLoop = RemoveSystem<Update.ScriptRunBehaviourUpdate>(playerLoop, typeof(UnityEventHolderUpdate));
            playerLoop = RemoveSystem<FixedUpdate.ScriptRunBehaviourFixedUpdate>(playerLoop, typeof(UnityEventHolderFixedUpdate));
            playerLoop = RemoveSystem<PreLateUpdate.ScriptRunBehaviourLateUpdate>(playerLoop, typeof(UnityEventHolderLateUpdate));

            PlayerLoop.SetPlayerLoop(playerLoop);
            _isInjected = false;

            OnUpdate = null;
            OnLateUpdate = null;
            OnFixedUpdate = null;
        }

        private static PlayerLoopSystem InsertSystem<T>(PlayerLoopSystem playerLoop, Type insertType, Action callback)
        {
            if (playerLoop.subSystemList == null)
                return playerLoop;

            var subsystems = playerLoop.subSystemList;

            for (int i = 0; i < subsystems.Length; i++)
            {
                PlayerLoopSystem subSystem = subsystems[i];
                if (subSystem.type == typeof(T))
                {
                    var currentSubsystems = subSystem.subSystemList;
                    int currentLength = currentSubsystems?.Length ?? 0;

                    var newSubsystems = new PlayerLoopSystem[currentLength + 1];

                    if (currentSubsystems != null)
                    {
                        for (int j = 0; j < currentSubsystems.Length; j++)
                        {
                            newSubsystems[j] = currentSubsystems[j];
                        }
                    }

                    newSubsystems[^1] = new PlayerLoopSystem
                    {
                        type = insertType,
                        updateDelegate = () => callback?.Invoke()
                    };

                    subSystem.subSystemList = newSubsystems;
                    break;
                }

                if (subSystem.subSystemList != null)
                {
                    subsystems[i] = InsertSystem<T>(subSystem, insertType, callback);
                }
            }

            playerLoop.subSystemList = subsystems;
            return playerLoop;
        }

        private static PlayerLoopSystem RemoveSystem<T>(PlayerLoopSystem playerLoop, Type removeType)
        {
            if (playerLoop.subSystemList == null)
                return playerLoop;

            var subsystems = playerLoop.subSystemList;

            for (int i = 0; i < subsystems.Length; i++)
            {
                PlayerLoopSystem subSystem = subsystems[i];
                if (subSystem.type == typeof(T))
                {
                    if (subSystem.subSystemList == null)
                        continue;

                    var filtered = new System.Collections.Generic.List<PlayerLoopSystem>();
                    
                    foreach (var subsystem in subSystem.subSystemList)
                    {
                        if (subsystem.type != removeType)
                        {
                            filtered.Add(subsystem);
                        }
                    }

                    subSystem.subSystemList = filtered.Count > 0 ? filtered.ToArray() : null;
                    break;
                }

                if (subSystem.subSystemList != null)
                {
                    subsystems[i] = RemoveSystem<T>(subSystem, removeType);
                }
            }

            playerLoop.subSystemList = subsystems;
            return playerLoop;
        }

        private struct UnityEventHolderUpdate
        {
        }

        private struct UnityEventHolderFixedUpdate
        {
        }

        private struct UnityEventHolderLateUpdate
        {
        }
    }
}