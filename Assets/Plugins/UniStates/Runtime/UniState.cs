using System;
using Cysharp.Threading.Tasks;

namespace UniStates
{
       public abstract partial class UniState : IDisposable
       {
              public bool IsRunning { get; private set; }

              protected virtual void Enter()
              {
              }

              protected virtual void Exit()
              {
              }

              protected virtual UniTask EnterAsync()
              {
                     return UniTask.CompletedTask;
              }

              protected virtual UniTask ExitAsync()
              {
                     return UniTask.CompletedTask;
              }

              public virtual void Dispose()
              {
              }
       }
}