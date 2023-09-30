using Assessment_BE_Engineer_3_Web.Models;

namespace Assessment_BE_Engineer_3_Web.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
