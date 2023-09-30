namespace Assessment_BE_Engineer_3_API.Models.DTO
{
    public class FileUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
