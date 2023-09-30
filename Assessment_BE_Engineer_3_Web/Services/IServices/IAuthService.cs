using Assessment_BE_Engineer_3_Web.Models.DTO;

namespace Assessment_BE_Engineer_3_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
    }
}
