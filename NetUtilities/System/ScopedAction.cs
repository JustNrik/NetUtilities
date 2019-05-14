using System.Threading.Tasks;
#nullable enable
namespace System
{
    public readonly ref struct ScopedAction
    {
        private readonly Action _undoAction;

        public ScopedAction(Action startAction, Action undoAction)
        {
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            startAction();
        }

        public void Dispose()
            => _undoAction();


        public ValueTask DisposeAsync()
        {
            _undoAction();
            return new ValueTask();
        }
    }

    public readonly ref struct ScopedAction<T>
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

        public void Dispose()
            => _undoAction(_obj);


        public ValueTask DisposeAsync()
        {
            _undoAction(_obj);
            return new ValueTask();
        }
    }
}
