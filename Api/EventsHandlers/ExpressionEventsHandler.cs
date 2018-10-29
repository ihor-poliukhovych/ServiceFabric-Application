using System.Runtime.Caching;
using Expression.Interfaces;

namespace Api.Controllers
{
    class ExpressionEventsHandler : IExpressionEvents
    {
        public void ProcessCompleted(string expression, ExpressionPart[] variables)
        {
            MemoryCache.Default.Set(new CacheItem("result:" + expression, variables), new CacheItemPolicy());
        }

        public void ProgressUpdated(string expression, decimal progress)
        {
            MemoryCache.Default.Set(new CacheItem("progress:" + expression, progress), new CacheItemPolicy());
        }
    }
}
