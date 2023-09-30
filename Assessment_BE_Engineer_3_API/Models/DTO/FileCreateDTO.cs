using System.ComponentModel.DataAnnotations.Schema;

namespace Assessment_BE_Engineer_3_API.Models.DTO
{
    public class FileCreateDTO
    {
        public string Name { get; set; }
		public IFormFile? FormFile { get; set; }
	}
}
