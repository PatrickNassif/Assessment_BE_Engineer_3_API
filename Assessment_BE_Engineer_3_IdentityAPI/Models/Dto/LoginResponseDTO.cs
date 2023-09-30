namespace Assessment_BE_Engineer_3_IdentityAPI.Models.Dto
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
