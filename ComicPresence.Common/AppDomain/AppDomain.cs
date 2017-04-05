using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using mscoree;

namespace ComicPresence.Common.AppDomain
{
    public static class AppDomain
    {
        public static IList<System.AppDomain> GetAppDomains()
        {
            IList<System.AppDomain> _IList = new List<System.AppDomain>();
            IntPtr enumHandle = IntPtr.Zero;
            ICorRuntimeHost host = new mscoree.CorRuntimeHost();
            try
            {
                host.EnumDomains(out enumHandle);
                object domain = null;
                while (true)
                {
                    host.NextDomain(enumHandle, out domain);
                    if (domain == null) break;
                    System.AppDomain appDomain = (System.AppDomain)domain;
                    _IList.Add(appDomain);
                }
                return _IList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
            finally
            {
                host.CloseEnum(enumHandle);
                Marshal.ReleaseComObject(host);
            }
        }
    }
}
