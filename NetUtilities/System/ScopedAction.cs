using System.Threading.Tasks;
#nullable enable
namespace System
{
    public readonly ref struct ScopedAction
    {
        private readonly Action _undoAction;

        public static ScopedAction CreateScope(Action startAction, Action undoAction)
            => new ScopedAction(startAction, undoAction);

        private ScopedAction(Action startAction, Action undoAction)
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

        public static ScopedAction<T> CreateScope(T obj, Action<T> startAction, Action<T> undoAction)
            => new ScopedAction<T>(ref obj, startAction, undoAction);

        private ScopedAction(ref T obj, Action<T> startAction, Action<T> undoAction)
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

    public readonly ref struct ScopedAction<T1, T2>
    {
        private readonly Action<T1, T2> _undoAction;
        private readonly T1 _obj;
        private readonly T2 _obj2;

        public static ScopedAction<T1, T2> CreateScope(T1 obj, T2 obj2, Action<T1, T2> startAction, Action<T1, T2> undoAction)
            => new ScopedAction<T1, T2>(ref obj, ref obj2, startAction, undoAction);

        private ScopedAction(ref T1 obj, ref T2 obj2, Action<T1, T2> startAction, Action<T1, T2> undoAction)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            _obj = obj;
            _obj2 = obj2;
            startAction(obj, obj2);
        }

        public void Dispose()
            => _undoAction(_obj, _obj2);

        public ValueTask DisposeAsync()
        {
            _undoAction(_obj, _obj2);
            return new ValueTask();
        }

    }

    public readonly ref struct ScopedAction<T1, T2, T3>
    {
        private readonly Action<T1, T2, T3> _undoAction;
        private readonly T1 _obj;
        private readonly T2 _obj2;
        private readonly T3 _obj3;

        public static ScopedAction<T1, T2, T3> CreateScope(T1 obj, T2 obj2, T3 obj3, Action<T1, T2, T3> startAction, Action<T1, T2, T3> undoAction)
            => new ScopedAction<T1, T2, T3>(ref obj, ref obj2, ref obj3, startAction, undoAction);

        private ScopedAction(ref T1 obj, ref T2 obj2, ref T3 obj3, Action<T1, T2, T3> startAction, Action<T1, T2, T3> undoAction)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            _obj = obj;
            _obj2 = obj2;
            _obj3 = obj3;
            startAction(obj, obj2, obj3);
        }

        public void Dispose()
            => _undoAction(_obj, _obj2, _obj3);

        public ValueTask DisposeAsync()
        {
            _undoAction(_obj, _obj2, _obj3);
            return new ValueTask();
        }
    }
}