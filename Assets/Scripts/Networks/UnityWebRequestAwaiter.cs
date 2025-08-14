using UnityEngine;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.Networking;

namespace Fantasy3D
{
    public struct UnityWebRequestAwaiter : INotifyCompletion
    {
        UnityWebRequestAsyncOperation asyncOp;
        Action conti;

        public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
        {
            this.asyncOp = asyncOp;
            conti = null;
        }

        public bool IsCompleted { get { return asyncOp.isDone; } }
        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            this.conti = continuation;
            asyncOp.completed += OnRequestCompleted;
        }

        private void OnRequestCompleted(AsyncOperation operation)
        {
            conti.Invoke();
        }

        
    }
    public static class ExtensionMethods
    {
        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp) 
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}
