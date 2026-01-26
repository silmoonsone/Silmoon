#if NET
#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Silmoon.Threading
{
    public sealed class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim semaphoreSlim = new(1, 1);

        public static AsyncLock Create() => new();

        public async ValueTask<Releaser> LockAsync(CancellationToken cancellationToken = default)
        {
            await semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);
            return new Releaser(semaphoreSlim);
        }

        public bool TryLock(out Releaser? releaser)
        {
            if (semaphoreSlim.Wait(0))
            {
                releaser = new Releaser(semaphoreSlim);
                return true;
            }
            releaser = null;
            return false;
        }
        public Releaser? TryLock()
        {
            if (semaphoreSlim.Wait(0))
                return new Releaser(semaphoreSlim);
            return null;
        }
        public async ValueTask<Releaser?> TryLockAsync(TimeSpan timeout, CancellationToken ct = default)
        {
            if (timeout < TimeSpan.Zero && timeout != Timeout.InfiniteTimeSpan)
                throw new ArgumentOutOfRangeException(nameof(timeout));

            if (await semaphoreSlim.WaitAsync(timeout, ct).ConfigureAwait(false))
                return new Releaser(semaphoreSlim);
            return null;
        }


        public void Dispose() => semaphoreSlim.Dispose();

        public sealed class Releaser : IDisposable
        {
            private SemaphoreSlim? semaphoreSlim;
            internal Releaser(SemaphoreSlim sem) => semaphoreSlim = sem;
            public void Dispose() => Interlocked.Exchange(ref semaphoreSlim, null)?.Release();
        }
    }
}
#endif