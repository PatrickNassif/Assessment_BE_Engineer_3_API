using Assessment_BE_Engineer_3_Utility;
using Assessment_BE_Engineer_3_Web.Models;
using Assessment_BE_Engineer_3_Web.Models.DTO;
using Assessment_BE_Engineer_3_Web.Services.IServices;

namespace Assessment_BE_Engineer_3_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string identityUrl;

        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            identityUrl = configuration.GetValue<string>("ServiceUrls:IdentityAPI");

        }

        public Task<T> LoginAsync<T>(LoginRequestDTO objToCreate)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = objToCreate,
                Url = identityUrl + "/api/UsersAuth/login"
            });
        }
    }
}
