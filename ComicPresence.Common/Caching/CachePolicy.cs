﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicPresence.Common.Caching
{
    public sealed class CachePolicy
    {
        private readonly TimeSpan expiresAfter;
        private readonly bool renewLeaseOnAccess;

        public CachePolicy(TimeSpan expiresAfter, bool renewLeaseOnAccess = false)
        {
            this.expiresAfter = expiresAfter;
            this.renewLeaseOnAccess = renewLeaseOnAccess;
        }

        public TimeSpan ExpiresAfter {  get { return this.expiresAfter; } }

        /// <summary>
        /// If specified, each read of the item from the cache will reset the expiration time.
        /// </summary>
        public bool RenewLeaseOnAccess {  get { return this.renewLeaseOnAccess; } }
    }
}
