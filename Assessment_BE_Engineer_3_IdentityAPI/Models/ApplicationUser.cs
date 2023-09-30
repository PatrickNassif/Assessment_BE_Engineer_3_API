using Microsoft.AspNetCore.Identity;

namespace Assessment_BE_Engineer_3_IdentityAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
