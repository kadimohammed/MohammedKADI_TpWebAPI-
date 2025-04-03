using Service.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByUsernameAsync(string username);

        Task<UserDto> RegisterUserAsync(string username, string email, string password);

        bool VerifyPassword(UserDto user, string password);
    }
}
