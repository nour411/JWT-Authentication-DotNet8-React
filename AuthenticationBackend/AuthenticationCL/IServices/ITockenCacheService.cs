using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCL.IServices
{
    public interface ITokenCacheService
    {
        // Refresh token
        Task StoreRefreshTokenAsync(Guid userId, string token, TimeSpan ttl);
        Task<string?> GetRefreshTokenAsync(Guid userId);
        Task RemoveRefreshTokenAsync(Guid userId);

        // Password reset token
        Task StorePasswordResetTokenAsync(string email, string token, TimeSpan ttl);
        Task<string?> GetPasswordResetTokenAsync(string email);
        Task RemovePasswordResetTokenAsync(string email);
    }
}
