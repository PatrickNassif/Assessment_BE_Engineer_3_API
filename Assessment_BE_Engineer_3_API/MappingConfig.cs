using Assessment_BE_Engineer_3_API.Models;
using Assessment_BE_Engineer_3_API.Models.DTO;
using AutoMapper;

namespace Assessment_BE_Engineer_3_API
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<FileDTO, FileModel>();
                config.CreateMap<FileModel, FileDTO>();

                config.CreateMap<FileCreateDTO, FileModel>();
                config.CreateMap<FileModel, FileCreateDTO>();

                config.CreateMap<FileUpdateDTO, FileModel>();
                config.CreateMap<FileModel, FileUpdateDTO>();
            });

            return mappingConfig;
        }
    }
}
