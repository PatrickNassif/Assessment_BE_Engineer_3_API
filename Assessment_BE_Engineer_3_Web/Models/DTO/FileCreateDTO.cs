
namespace Assessment_BE_Engineer_3_Web.Models.DTO
{
    public class FileCreateDTO
    {
        public string Name { get; set; }
		public IFormFile? FormFile { get; set; }
	}
}
