using AuthenticationCL.IServices;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCL.Services
{
    public class TokenCacheService : ITokenCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly string _prefix;

        public TokenCacheService(IDistributedCache cache, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _cache = cache;
            _prefix = config.GetValue<string>("Redis:InstanceName") ?? "AuthApp_";
        }

        private string KeyRefresh(Guid userId) => $"{_prefix}refresh:{userId}";
        private string KeyReset(string email) => $"{_prefix}reset:{email.ToLowerInvariant()}";

        public async Task StoreRefreshTokenAsync(Guid userId, string token, TimeSpan ttl)
        {
            var opts = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl };
            await _cache.SetStringAsync(KeyRefresh(userId), token, opts);
        }

        public async Task<string?> GetRefreshTokenAsync(Guid userId)
            => await _cache.GetStringAsync(KeyRefresh(userId));

        public async Task RemoveRefreshTokenAsync(Guid userId)
            => await _cache.RemoveAsync(KeyRefresh(userId));

        public async Task StorePasswordResetTokenAsync(string email, string token, TimeSpan ttl)
        {
            var opts = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl };
            await _cache.SetStringAsync(KeyReset(email), token, opts);
        }

        public async Task<string?> GetPasswordResetTokenAsync(string email)
            => await _cache.GetStringAsync(KeyReset(email));

        public async Task RemovePasswordResetTokenAsync(string email)
            => await _cache.RemoveAsync(KeyReset(email));
    }

}
