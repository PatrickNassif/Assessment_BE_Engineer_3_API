using Assessment_BE_Engineer_3_Web.Models.DTO;
using AutoMapper;

namespace Assessment_BE_Engineer_3_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //source -> target
            CreateMap<FileDTO, FileCreateDTO>().ReverseMap();
            CreateMap<FileDTO, FileUpdateDTO>().ReverseMap();
        }
    }
}
