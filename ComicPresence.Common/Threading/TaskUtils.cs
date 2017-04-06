using System;
using System.Threading;
using System.Threading.Tasks;
using ComicPresence.Common.Logging;

namespace ComicPresence.Common.Threading
{
    public class TaskUtils
    {
        /// <summary>
        /// Wrapper that takes care of exception handling. Will keep existing worker process alive during IIS recycles (same behavior as outstanding web requests/connections). 
        /// </summary>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static void RunFireAndForget(Func<Task> fn)
        {
            System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(
                async delegate (CancellationToken token)
                {
                    try
                    {
                        await fn().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        HandleFireAndForgetException(ex);
                    }
                }
            );
        }

        private static void HandleFireAndForgetException(Exception ex)
        {
            LogManager.Instance.LogError("Error in fire-and-forget method", ex);
        }
    }
}
