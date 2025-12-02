using AuthenticationCL.IServices;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCL.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IDistributedCache _cache;

        public PasswordResetService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task GenerateResetTokenAsync(string email)
        {
            string token = Guid.NewGuid().ToString("N");

            await _cache.SetStringAsync(
                $"pwdReset:{email}",
                token,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                });

            string link = $"https://your-frontend.com/reset-password?email={email}&token={token}";

           
        }

        public async Task<bool> ValidateResetTokenAsync(string email, string token)
        {
            var saved = await _cache.GetStringAsync($"pwdReset:{email}");
            return saved == token;
        }
    }
}
