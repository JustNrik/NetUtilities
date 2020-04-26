using System.Collections.Immutable;
using System.Threading;

namespace System
{
    /// <summary>
    /// This class gives a default <see cref="SynchronizationContext"/> for contextless application like Console. 
    /// Useful when you have <see langword="async"/> <see langword="void"/> methods as they exceptions cannot be caught in a contexless scenario.
    /// </summary>
    public sealed class DefaultSynchronizationContext : SynchronizationContext
    {
        private readonly object _lock = new object();
        private ImmutableArray<Exception> _exceptions = ImmutableArray.Create<Exception>();

        /// <summary>
        /// Singleton instance of a <see cref="DefaultSynchronizationContext"/>.
        /// </summary>
        public static DefaultSynchronizationContext Shared { get; } = new DefaultSynchronizationContext();

        /// <inheritdoc/>
        public override void Send(SendOrPostCallback callback, object? state)
        {
            try
            {
                callback(state);
            }
            catch (Exception ex)
            {
                lock (_lock)
                    _exceptions = _exceptions.Add(ex);
            }
        }

        /// <inheritdoc/>
        public override void Post(SendOrPostCallback callback, object? state)
            => Send(callback, state);

        /// <inheritdoc/>
        public override void OperationCompleted()
        {
            lock (_lock)
            {
                if (_exceptions.Length == 0)
                    return;

                SynchronizationContextHelper.OnCapturedException(
                    this,
                    new CapturedExceptionEventArgs(
                        _exceptions.Length == 1
                        ? _exceptions[0]
                        : new AggregateException(_exceptions)));

                _exceptions = _exceptions.Clear();
            }
        }

        /// <inheritdoc/>
        public override SynchronizationContext CreateCopy()
            => new DefaultSynchronizationContext();

        /// <inheritdoc/>
        public override string ToString()
            => nameof(DefaultSynchronizationContext);
    }
}
