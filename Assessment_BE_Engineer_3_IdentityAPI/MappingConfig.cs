using Assessment_BE_Engineer_3_IdentityAPI.Models;
using Assessment_BE_Engineer_3_IdentityAPI.Models.Dto;
using AutoMapper;

namespace Assessment_BE_Engineer_3_IdentityAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
