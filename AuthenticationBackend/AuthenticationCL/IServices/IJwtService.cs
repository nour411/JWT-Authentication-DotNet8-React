using AuthenticationCL.Domain;
using AuthenticationCL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCL.IServices
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
