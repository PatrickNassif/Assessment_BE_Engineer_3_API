using System.ComponentModel.DataAnnotations.Schema;

namespace Assessment_BE_Engineer_3_API.Models.DTO
{
    public class FileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Location { get; set; }
        public DateTime UploadDate { get; set; }
		public IFormFile FormFile { get; set; }
	}
}
