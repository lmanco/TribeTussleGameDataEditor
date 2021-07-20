using AutoMapper;
using TribeTussleGameDataEditor.API.Controllers.Util;
using TribeTussleGameDataEditor.API.DAL.Models;
using TribeTussleGameDataEditor.API.DAL.Repositories;
using TribeTussleGameDataEditor.API.Util;
using Microsoft.Extensions.Options;

namespace TribeTussleGameDataEditor.API.DAL
{
    public interface IDALInit
    {
        void Init();
    }

    public class DALInit : IDALInit
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly UserRequestDTO _adminUser;

        public DALInit(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, IOptionsMonitor<UserRequestDTO> adminUserConfig)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _adminUser = adminUserConfig.CurrentValue;
        }

        public void Init()
        {
            if (_userRepository.GetByEmail(_adminUser.Email).Result == null)
            {
                User user = _mapper.Map<User>(_adminUser);
                user.Email = user.Email;
                user.Username = user.Username;
                user.PasswordHash = _passwordHasher.HashPassword(_adminUser.Password);
                user.Role = UserRole.Admin;
                user.Verified = true;
                _userRepository.Create(user).Wait();
            }
        }
    }
}
