using System.Threading;
using System.Threading.Tasks;
#nullable enable
namespace System
{
    public readonly ref struct ActionScope
    {
        private readonly Action _undoAction;
        private readonly CancellationTokenSource _cts;

        public static ActionScope Create(Action startAction, Action undoAction)
            => new ActionScope(startAction, undoAction);

        private ActionScope(Action startAction, Action undoAction)
        {
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            _cts = new CancellationTokenSource();
            startAction();
        }

        public void Cancel()
            => _cts.Cancel();

        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction();

            _cts.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction();

            _cts.Dispose();
            return new ValueTask();
        }
    }

    public readonly ref struct ActionScope<T>
    {
        private readonly Action<T> _undoAction;
        private readonly CancellationTokenSource _cts;
        private readonly T _obj;

        public static ActionScope<T> Create(T obj, Action<T> startAction, Action<T> undoAction)
            => new ActionScope<T>(ref obj, startAction, undoAction);

        private ActionScope(ref T obj, Action<T> startAction, Action<T> undoAction)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            _obj = obj;
            _cts = new CancellationTokenSource();
            startAction(obj);
        }

        public void Cancel()
            => _cts.Cancel();

        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction(_obj);

            _cts.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction(_obj);

            _cts.Dispose();
            return new ValueTask();
        }
    }

    public readonly ref struct ActionScope<T1, T2>
    {
        private readonly Action<T1, T2> _undoAction;
        private readonly CancellationTokenSource _cts;
        private readonly T1 _obj;
        private readonly T2 _obj2;

        public static ActionScope<T1, T2> Create(T1 obj, T2 obj2, Action<T1, T2> startAction, Action<T1, T2> undoAction)
            => new ActionScope<T1, T2>(ref obj, ref obj2, startAction, undoAction);

        private ActionScope(ref T1 obj, ref T2 obj2, Action<T1, T2> startAction, Action<T1, T2> undoAction)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (obj2 is null) throw new ArgumentNullException(nameof(obj2));
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            _obj = obj;
            _obj2 = obj2;
            _cts = new CancellationTokenSource();
            startAction(obj, obj2);
        }

        public void Cancel()
            => _cts.Cancel();

        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction(_obj, _obj2);

            _cts.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction(_obj, _obj2);

            _cts.Dispose();
            return new ValueTask();
        }

    }

    public readonly ref struct ActionScope<T1, T2, T3>
    {
        private readonly Action<T1, T2, T3> _undoAction;
        private readonly CancellationTokenSource _cts;
        private readonly T1 _obj;
        private readonly T2 _obj2;
        private readonly T3 _obj3;

        public static ActionScope<T1, T2, T3> Create(T1 obj, T2 obj2, T3 obj3, Action<T1, T2, T3> startAction, Action<T1, T2, T3> undoAction)
            => new ActionScope<T1, T2, T3>(ref obj, ref obj2, ref obj3, startAction, undoAction);

        private ActionScope(ref T1 obj, ref T2 obj2, ref T3 obj3, Action<T1, T2, T3> startAction, Action<T1, T2, T3> undoAction)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (obj2 is null) throw new ArgumentNullException(nameof(obj2));
            if (obj3 is null) throw new ArgumentNullException(nameof(obj3));
            if (startAction is null) throw new ArgumentNullException(nameof(startAction));
            if (undoAction is null) throw new ArgumentNullException(nameof(undoAction));

            _undoAction = undoAction;
            _obj = obj;
            _obj2 = obj2;
            _obj3 = obj3;
            _cts = new CancellationTokenSource();
            startAction(obj, obj2, obj3);
        }

        public void Cancel()
            => _cts.Cancel();

        public void Dispose()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction(_obj, _obj2, _obj3);

            _cts.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            if (!_cts.IsCancellationRequested)
                _undoAction(_obj, _obj2, _obj3);

            _cts.Dispose();
            return new ValueTask();
        }
    }
}