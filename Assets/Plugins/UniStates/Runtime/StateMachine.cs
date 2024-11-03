using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace UniStates
{
       public abstract partial class UniState
       {
              public class StateMachine : IDisposable
              {
                     private readonly Dictionary<Type, UniState> _states = new();
                     [CanBeNull] private UniState _currentState;

                     public bool TryAddState<T>([CanBeNull] T state) where T : UniState
                     {
                            Type key = typeof(T);
                            return _states.TryAdd(key, state);
                     }

                     public void AddState<T>([CanBeNull] T state) where T : UniState
                     {
                            if(!TryAddState(state)) throw new Exception("Failed to Add State");
                     }

                     public async UniTask<bool> TrySwitchStateAsync(Type stateType)
                     {
                            if (_states.TryGetValue(stateType, out UniState state))
                            {
                                   if (_currentState != null)
                                   {
                                          _currentState.IsRunning = false;
                                          await _currentState.ExitAsync()!;
                                          _currentState.Exit();
                                   }

                                   if (state != null)
                                   {
                                          _currentState = state;
                                          await _currentState.EnterAsync()!;
                                          _currentState.Enter();
                                          _currentState.IsRunning = true;

                                          return true;
                                   }

                                   return false;
                            }

                            return false;
                     }

                     public async UniTask<bool> TrySwitchStateAsync<T>() where T : UniState
                     {
                            Type key = typeof(T);
                            return await TrySwitchStateAsync(key);
                     }

                     public async UniTaskVoid SwitchStateAsync(Type stateType)
                     {
                            if (!await TrySwitchStateAsync(stateType))
                                   throw new Exception("Failed to Switch State");
                     }

                     public async UniTaskVoid SwitchStateAsync<T>() where T : UniState
                     {
                            Type key = typeof(T);
                            if (!await TrySwitchStateAsync(key))
                                   throw new Exception("Failed to Switch State");
                     }

                     public bool TrySwitchState(Type stateType)
                     {
                            if (_states.TryGetValue(stateType, out UniState state))
                            {
                                   if (_currentState != null)
                                   {
                                          _currentState.IsRunning = false;
                                          _currentState.Exit();
                                   }

                                   if (state != null)
                                   {
                                          _currentState = state;
                                          _currentState.Enter();
                                          _currentState.IsRunning = true;

                                          return true;
                                   }

                                   return false;
                            }

                            return false;
                     }

                     public bool TrySwitchState<T>() where T : UniState
                     {
                            Type key = typeof(T);
                            return TrySwitchState(key);
                     }

                     public void SwitchState(Type stateType)
                     {
                            if (!TrySwitchState(stateType))
                                   throw new Exception("Failed to Switch State");
                     }

                     public void SwitchState<T>() where T : UniState
                     {
                            Type key = typeof(T);
                            if (!TrySwitchState(key))
                                   throw new Exception("Failed to Switch State");
                     }

                     public void Dispose()
                     {
                            _currentState?.Dispose();
                     }
              }
       }
}