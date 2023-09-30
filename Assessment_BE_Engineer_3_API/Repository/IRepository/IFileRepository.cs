using Assessment_BE_Engineer_3_API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Assessment_BE_Engineer_3_API.Repository.IRepository
{
    public interface IFileRepository : IRepository<FileModel>
    {
		public string RemoveSpecialCharactersAndSpaces(string input);
		public string FormatFileSize(long fileSizeBytes);

	}
}
