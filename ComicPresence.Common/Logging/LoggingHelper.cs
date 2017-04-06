using System;

namespace ComicPresence.Common.Logging
{
    public static class LoggingHelper
    {
        public const string cErrorNoneStr = "Error";

        /// <summary>
        /// Handles an error that occurs in Global.asax -> Application_Error
        /// </summary>
        public static void HandleGlobalAsaxError(Exception ex)
        {
            try
            {
                ComicPresence.Common.Logging.LogManager.Instance.LogError(String.Format("Exception in Global.asax\r\n\r\n{0}", ex.Message.ToString()), ex);
            }
            catch (Exception ex2)
            {
                EventLogHelper.LogEvent(String.Format("Exception while logging error in Global.asax: {0}\r\n\r\nOriginal error: {1}",
                    ex2.ToString(),
                    ex.ToString()));
            }
        }
    }
}
