using AutoMapper;
using TribeTussleGameDataEditor.API.DAL.Models;

namespace TribeTussleGameDataEditor.API.Util
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration GetConfig()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<UserRequestDTO, User>();
                config.CreateMap<User, UserResponseDTO>();
                config.CreateMap<GameDataRequestDTO, GameData>();
            });
        }
    }
}
