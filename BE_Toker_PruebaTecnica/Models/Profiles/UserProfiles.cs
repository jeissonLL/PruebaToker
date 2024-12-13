using AutoMapper;
using BE_Toker_PruebaTecnica.Models.DTO;

namespace BE_Toker_PruebaTecnica.Models.Profiles
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
