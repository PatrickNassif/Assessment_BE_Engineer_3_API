using Assessment_BE_Engineer_3_API.Data;
using Assessment_BE_Engineer_3_API.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_BE_Engineer_3_API.Tests
{
    public class FileRepositoryTests
    {
        private FileRepository _fileRepository;
        private DbContextOptions<ApplicationDbContext> options;
        ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            // Arrange: Create an instance of FileRepository with a mock ApplicationDbContext
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "temp_db").Options;
            _context = new ApplicationDbContext(options);
            _fileRepository = new FileRepository(_context);
        }

        [Test]
        public void RemoveSpecialCharactersAndSpaces_ReturnsString()
        {
            //arrange
            string expectedValue = "name1";
            // Act
            string result = _fileRepository.RemoveSpecialCharactersAndSpaces("name_1$");
            // Assert
            Assert.AreEqual(expectedValue, result);
        }

        [Test]
        [TestCase(2048, ExpectedResult = "2.00 KB")]
        [TestCase(2 * 1024 * 1024, ExpectedResult = "2.00 MB")]
        public string FormatFileSize_ReturnCorrectSizeInKBOrMB(long fileSizeBytes)
        {
            // Act
            string result = _fileRepository.FormatFileSize(fileSizeBytes);
            // Assert
            return result;
        }

    }
}

