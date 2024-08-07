using System;
using System.Threading;

namespace ThreadPoolExercises.Core
{
    public class ThreadingHelpers
    {
        public static void ExecuteOnThread(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            // * Create a thread and execute there `action` given number of `repeats` - waiting for the execution!
            //   HINT: you may use `Join` to wait until created Thread finishes
            // * In a loop, check whether `token` is not cancelled
            // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)
            try
            {
                //if (errorAction != null)
                //{

                //    AppDomain currentDomain = AppDomain.CurrentDomain;
                //    currentDomain.UnhandledException += new UnhandledExceptionEventHandler(errorAction);
                //}
                var isThrowed = false;
                for (var i = 0; i < repeats; i++)
                {

                    if (isThrowed)
                    {
                        break;
                    }
                    token.ThrowIfCancellationRequested();
                    Thread newThread = new Thread(new ThreadStart(() =>
                    {
                        try
                        {

                            action();
                        }
                        catch (Exception ex)
                        {
                            errorAction?.Invoke(ex);
                            isThrowed = true;
                        }
                    }));

                    newThread.Start();
                    newThread.Join();
                }
            }
            catch (Exception ex)
            {
                if (errorAction != null) { errorAction(ex); }
            }
        }



        public static void ExecuteOnThreadPool(Action action, int repeats, CancellationToken token = default, Action<Exception>? errorAction = null)
        {
            //while (true)
            //{

            //    // * Queue work item to a thread pool that executes `action` given number of `repeats` - waiting for the execution!
            //    //   HINT: you may use `AutoResetEvent` to wait until the queued work item finishes
            //    // * In a loop, check whether `token` is not cancelled
            //    // * If an `action` throws and exception (or token has been cancelled) - `errorAction` should be invoked (if provided)
            //}
            try
            {
                //if (errorAction != null)
                //{

                //    AppDomain currentDomain = AppDomain.CurrentDomain;
                //    currentDomain.UnhandledException += new UnhandledExceptionEventHandler(errorAction);
                //}
                var isThrowed = false;
                for (var i = 0; i < repeats; i++)
                {

                    if (isThrowed)
                    {
                        break;
                    }
                    token.ThrowIfCancellationRequested();
                    var isFinished = false;
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {

                            action();
                        }
                        catch (Exception ex)
                        {
                            errorAction?.Invoke(ex);
                            isThrowed = true;
                        }
                        isFinished = true;
                    });
                    while (!isFinished)
                    {
                        Thread.SpinWait(10);
                    }

                }
            }
            catch (Exception ex)
            {
                if (errorAction != null) { errorAction(ex); }
            }

        }
    }
}
