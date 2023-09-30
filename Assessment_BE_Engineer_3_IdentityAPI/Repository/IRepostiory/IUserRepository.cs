using Assessment_BE_Engineer_3_IdentityAPI.Models;
using Assessment_BE_Engineer_3_IdentityAPI.Models.Dto;

namespace Assessment_BE_Engineer_3_IdentityAPI.Repository.IRepostiory
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
    }
}
