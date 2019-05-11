using System.Threading.Tasks;

namespace System
{
    public readonly struct ScopedAction : IDisposable, IAsyncDisposable
    {
        private readonly Action _undoAction;

        public ScopedAction(Action startAction, Action undoAction)
        {
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            startAction();
        }

        void IDisposable.Dispose()
        {
            if (!(_undoAction is null))
                _undoAction();
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            ((IDisposable)this).Dispose();
            return new ValueTask();
        }
    }

    public readonly struct ScopedAction<T> : IDisposable, IAsyncDisposable
    {
        private readonly Action<T> _undoAction;
        private readonly T _obj;

        public ScopedAction(T obj, Action<T> startAction, Action<T> undoAction)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            _obj = obj;
            startAction(obj);
        }

        void IDisposable.Dispose()
        {
            if (!(_undoAction is null || _obj is null))
                _undoAction(_obj);
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            ((IDisposable)this).Dispose();
            return new ValueTask();
        }
    }
}
