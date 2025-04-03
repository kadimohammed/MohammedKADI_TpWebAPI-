using AutoMapper;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using Service.DTOS;
using Service.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashPasswordService _hash;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IHashPasswordService hash)
        {
            _userRepository = userRepository;
            _hash = hash;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            User user = await _userRepository.GetUserByUsernameAsync(username);
            if (user is not null)
            {
                return _mapper.Map<UserDto>(user);
            }
            throw new InvalidUserException("User not found");
        }

        public async Task<UserDto> RegisterUserAsync(string username, string email, string password)
        {
            User user = new User();
            user.Username = username;
            user.Email = email;
            user.PasswordHash = _hash.HashPassword(password);
            await _userRepository.RegisterUserAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public bool VerifyPassword(UserDto user, string password)
        {
            if(user == null || password == null)
            {
                return false;
            }
            return _hash.VerifyPassword(password,user.PasswordHash);
        }
    }
}
