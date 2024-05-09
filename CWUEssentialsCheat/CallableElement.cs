using System;
using JetBrains.Annotations;

namespace MonoInjectionTemplate
{

    public abstract class CallableElement
    {
        [CanBeNull] protected Action<CallableElement> _callback;

        public abstract void Draw();

        public void On(Action<CallableElement> cb)
        {
            _callback = cb;
        }

        internal void Call()
        {
            _callback?.Invoke(this);
        }
    }
}