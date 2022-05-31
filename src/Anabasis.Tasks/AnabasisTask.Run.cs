namespace Anabasis.Tasks;

public readonly partial struct AnabasisTask
{
    /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
    public static async AnabasisTask RunOnThreadPool(Action action, bool configureAwait = true,
        CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();

        await SwitchToThreadPool();

        cancellationToken.ThrowIfCancellationRequested();

        if (configureAwait) {
            try {
                action();
            }
            finally {
                await Yield();
            }
        } else {
            action();
        }

        cancellationToken.ThrowIfCancellationRequested();
    }

    /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
    public static async AnabasisTask RunOnThreadPool<TArg>(Action<TArg> action, TArg state, bool configureAwait = true,
        CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();

        await SwitchToThreadPool();

        cancellationToken.ThrowIfCancellationRequested();

        if (configureAwait) {
            try {
                action(state);
            }
            finally {
                await Yield();
            }
        } else {
            action(state);
        }

        cancellationToken.ThrowIfCancellationRequested();
    }

    /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
    public static async AnabasisTask RunOnThreadPool(Func<AnabasisTask> action, bool configureAwait = true,
        CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();

        await SwitchToThreadPool();

        cancellationToken.ThrowIfCancellationRequested();

        if (configureAwait) {
            try {
                await action();
            }
            finally {
                await Yield();
            }
        } else {
            await action();
        }

        cancellationToken.ThrowIfCancellationRequested();
    }

    /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
    public static async AnabasisTask RunOnThreadPool(Func<object, AnabasisTask> action, object state,
        bool configureAwait = true, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();

        await SwitchToThreadPool();

        cancellationToken.ThrowIfCancellationRequested();

        if (configureAwait) {
            try {
                await action(state);
            }
            finally {
                await Yield();
            }
        } else {
            await action(state);
        }

        cancellationToken.ThrowIfCancellationRequested();
    }

    /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
    public static async AnabasisTask<T> RunOnThreadPool<T>(Func<T> func, bool configureAwait = true,
        CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();

        await SwitchToThreadPool();

        cancellationToken.ThrowIfCancellationRequested();

        if (configureAwait) {
            try {
                return func();
            }
            finally {
                await Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        return func();
    }

    /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
    public static async AnabasisTask<T> RunOnThreadPool<T>(Func<AnabasisTask<T>> func, bool configureAwait = true,
        CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();

        await SwitchToThreadPool();

        cancellationToken.ThrowIfCancellationRequested();

        if (configureAwait) {
            try {
                return await func();
            }
            finally {
                cancellationToken.ThrowIfCancellationRequested();
                await Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        T result = await func();
        cancellationToken.ThrowIfCancellationRequested();
        return result;
    }

    /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
    public static async AnabasisTask<T> RunOnThreadPool<T, TArg>(Func<TArg, T> func, TArg state,
        bool configureAwait = true, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();

        await SwitchToThreadPool();

        cancellationToken.ThrowIfCancellationRequested();

        if (configureAwait) {
            try {
                return func(state);
            }
            finally {
                await Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        return func(state);
    }

    /// <summary>Run action on the threadPool and return to main thread if configureAwait = true.</summary>
    public static async AnabasisTask<T> RunOnThreadPool<T, TArg>(Func<TArg, AnabasisTask<T>> func, TArg state,
        bool configureAwait = true, CancellationToken cancellationToken = default) {
        cancellationToken.ThrowIfCancellationRequested();

        await SwitchToThreadPool();

        cancellationToken.ThrowIfCancellationRequested();

        if (configureAwait) {
            try {
                return await func(state);
            }
            finally {
                cancellationToken.ThrowIfCancellationRequested();
                await Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        T result = await func(state);
        cancellationToken.ThrowIfCancellationRequested();
        return result;
    }
}