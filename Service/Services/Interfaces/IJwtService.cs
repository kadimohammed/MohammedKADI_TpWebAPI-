using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userId);
        void AddRoles(params string[] roles);
    }
}
