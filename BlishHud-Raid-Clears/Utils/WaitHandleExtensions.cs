using System;
using System.Threading;
using System.Threading.Tasks;

namespace RaidClears.Utils;
public static class WaitHandleExtensions
{
    public static Task<bool> WaitOneAsync(this WaitHandle waitHandle, TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (waitHandle == null)
            throw new ArgumentNullException(nameof(waitHandle));

        if (cancellationToken.IsCancellationRequested) return Task.FromResult(true);

        var tcs = new TaskCompletionSource<bool>();

        RegisteredWaitHandle registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(
            waitHandle,
            callBack: (state, timedOut) => { tcs.TrySetResult(!timedOut); },
            state: null,
            timeout: timeout,
            executeOnlyOnce: true);

        cancellationToken.Register(() =>
        {
            if (registeredWaitHandle.Unregister(null))
            {
                tcs.SetCanceled();
            }
        });

        return tcs.Task.ContinueWith((continuationTask) =>
        {
            registeredWaitHandle.Unregister(waitObject: null);
            try
            {
                return continuationTask.Result;
            }
            catch
            {
                return false;
            }
        });
    }
}