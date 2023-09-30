using Assessment_BE_Engineer_3_Utility;
using Assessment_BE_Engineer_3_Web.Models;
using Assessment_BE_Engineer_3_Web.Models.DTO;
using Assessment_BE_Engineer_3_Web.Services.IServices;

namespace Assessment_BE_Engineer_3_Web.Services
{
    public class FileService : BaseService,IFileService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string fileUrl;

        public FileService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            fileUrl = configuration.GetValue<string>("ServiceUrls:FileAPI");
        }

        public Task<T> CreateAsync<T>(FileCreateDTO dto, string token)
        {
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Url = fileUrl + "/api/v1/files",
                Data = dto,
				Token = token
			});
		}

        public Task<T> DeleteAsync<T>(int id, string token)
        {
			return SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.DELETE,
				Url = fileUrl + "/api/v1/files/"+id,
				Token = token
			});
		}

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = fileUrl + "/api/v1/files",
                Token = token
            });
        }
       
        public Task<T> DownloadAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = fileUrl + "/api/v1/files/"+id,
                Token = token
            });
        }
    }
}
