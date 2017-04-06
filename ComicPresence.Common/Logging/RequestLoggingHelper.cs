using System;

namespace ComicPresence.Common.Logging
{
    public static class RequestLoggingHelper
    {
        public static void SetNewRequestId()
        {
            SetRequestId(Guid.NewGuid().ToString());
        }

        public static void SetRequestId(string requestId)
        {
            Set(LoggingConstants.cRequestIdCallContextKey, requestId);
        }

        public static string GetCurrentRequestId()
        {
            return Get<string>(LoggingConstants.cRequestIdCallContextKey);
        }

        public static void SetUserAccountId(int userAccountId)
        {
            Set(LoggingConstants.cUserAccountIdCallContextKey, userAccountId);
        }

        public static int? GetCurrentUserAccountId()
        {
            return Get<int?>(LoggingConstants.cUserAccountIdCallContextKey);
        }

        public static void SetServiceUsername(string serviceUsername)
        {
            Set(LoggingConstants.cSvcUsernameCallContextKey, serviceUsername);
        }

        public static string GetCurrentServiceUsername()
        {
            return Get<string>(LoggingConstants.cSvcUsernameCallContextKey);
        }

        private static void Set<T>(string key, T value)
        {
            System.Runtime.Remoting.Messaging.CallContext.LogicalSetData(key, value);
        }

        private static T Get<T>(string key)
        {
            object obj = System.Runtime.Remoting.Messaging.CallContext.LogicalGetData(key);
            return (T)obj;
        }
    }
}
