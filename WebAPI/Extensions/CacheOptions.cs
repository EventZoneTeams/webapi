using Microsoft.Extensions.Caching.Distributed;

namespace EventZone.WebAPI.Extensions;
public static class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration =>
        new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20) };
}