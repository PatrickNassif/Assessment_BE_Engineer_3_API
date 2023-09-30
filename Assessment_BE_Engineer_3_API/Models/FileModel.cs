using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assessment_BE_Engineer_3_API.Models
{
    public class FileModel
    {
        [Key]
        public int Id { get;set; }
        public string Name { get;set; }
        public string Size { get;set; }
        public string Location { get;set; }
        public DateTime UploadDate { get;set; } = DateTime.Now;
		[NotMapped]
		public IFormFile FormFile { get; set; }
	}
}
