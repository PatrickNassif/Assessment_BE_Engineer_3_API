using Assessment_BE_Engineer_3_Web.Models.DTO;

namespace Assessment_BE_Engineer_3_Web.Services.IServices
{
    public interface IFileService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> CreateAsync<T>(FileCreateDTO dto, string token);
        Task<T> DownloadAsync<T>(int id, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
