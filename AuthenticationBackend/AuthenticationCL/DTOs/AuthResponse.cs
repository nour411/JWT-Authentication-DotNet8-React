using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationCL.DTOs
{
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
