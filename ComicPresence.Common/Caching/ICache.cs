using System;
using System.Linq.Expressions;

namespace ComicPresence.Common.Caching
{
    public interface ICache
    {
        TResult InvokeCached<TResult>(Expression<Func<TResult>> expression, CachePolicy policy);
    }
}
