using Assessment_BE_Engineer_3_API.Data;
using Assessment_BE_Engineer_3_API.Models;
using Assessment_BE_Engineer_3_API.Repository.IRepository;
using System.Text.RegularExpressions;

namespace Assessment_BE_Engineer_3_API.Repository
{
    public class FileRepository : Repository<FileModel>, IFileRepository
    {
        private ApplicationDbContext _db;

        public FileRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

		public string RemoveSpecialCharactersAndSpaces(string input)
		{
			// Define a regular expression that matches anything that is not a letter or digit
			Regex regex = new Regex("[^a-zA-Z0-9]");
			// Use the regular expression to replace matches with an empty string
			string result = regex.Replace(input, "");
			return result;
		}

		public string FormatFileSize(long fileSizeBytes)
		{
			const int KbInBytes = 1024;
			const int MbInBytes = 1024 * 1024;

			if (fileSizeBytes < KbInBytes)
			{
				return $"{fileSizeBytes} B";
			}
			else if (fileSizeBytes < MbInBytes)
			{
				double sizeInKB = (double)fileSizeBytes / KbInBytes;
				return $"{sizeInKB:F2} KB";
			}
			else
			{
				double sizeInMB = (double)fileSizeBytes / MbInBytes;
				return $"{sizeInMB:F2} MB";
			}
		}

	}
}
