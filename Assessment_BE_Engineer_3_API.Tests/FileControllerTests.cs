using Assessment_BE_Engineer_3_API.Controllers;
using Assessment_BE_Engineer_3_API.Models;
using Assessment_BE_Engineer_3_API.Models.DTO;
using Assessment_BE_Engineer_3_API.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assessment_BE_Engineer_3_API.Tests
{
    public class FileControllerTests
    {
        private FileController _fileController;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IWebHostEnvironment> _hostEnvironmentMock;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _hostEnvironmentMock = new Mock<IWebHostEnvironment>();

            _fileController = new FileController(_unitOfWorkMock.Object, _mapperMock.Object, _hostEnvironmentMock.Object);
        }
        [Test]
        public async Task GetAllAsync_ShouldReturnOkStatusWithData_WhenFilesExist()
        {
            // Arrange
            var filesFromRepository = new List<FileModel>(); // Initialize your test data
            var mappedFiles = new List<FileDTO>(); // Initialize your expected DTOs
            _unitOfWorkMock.Setup(repo => repo.FileRepository.GetAllAsync(It.IsAny<Expression<Func<FileModel, bool>>>(), It.IsAny<string>())).ReturnsAsync(filesFromRepository);
            _mapperMock.Setup(mapper => mapper.Map<List<FileDTO>>(filesFromRepository)).Returns(mappedFiles);

            // Act
            var result = await _fileController.GetAllAsync();

            // Assert
            _unitOfWorkMock.Verify(repo => repo.FileRepository.GetAllAsync(It.IsAny<Expression<Func<FileModel, bool>>>(), It.IsAny<string>()), Times.Exactly(1));
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            var apiResponse = (APIResponse)okResult.Value;
            Assert.AreEqual(mappedFiles, apiResponse.Result);
        }

        [Test]
        public async Task CreateAsync_ValidFile_ReturnsOkAndFileCreated()
        {
            // Arrange
            var fileCreateDTO = new FileCreateDTO
            {
                Name = "Test Created File",
                FormFile = CreateMockFormFile("test.xlsx")
            };
            var fileModel = new FileModel
            {
                Name = "Test Created File",
                FormFile = CreateMockFormFile("test.xlsx")
            };
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            // Set up mock behavior, including the AddAsync method
            unitOfWorkMock.Setup(repo => repo.FileRepository.AddAsync(It.IsAny<FileModel>()));
            unitOfWorkMock.Setup(repo => repo.FileRepository.RemoveSpecialCharactersAndSpaces(It.IsAny<string>())).Returns("TestCreatedFile");
            unitOfWorkMock.Setup(repo => repo.FileRepository.FormatFileSize(It.IsAny<long>())).Returns("2 KB");
            var hostEnvironmentMock = new Mock<IWebHostEnvironment>();
            // Set up mock behavior for WebRootPath
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(mapper => mapper.Map<FileModel>(fileCreateDTO)).Returns(fileModel);
            // Set up mock behavior for mapper
            var controller = new FileController(unitOfWorkMock.Object, mapperMock.Object, hostEnvironmentMock.Object);
            // Act
            var result = await controller.CreateAsync(fileCreateDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            // Check whether the file was created at the expected location
            var expectedFilePath = Path.Combine(@"uploads", fileModel.Name);
            Assert.IsTrue(File.Exists(expectedFilePath));
        }

        [Test]
        public async Task DownloadAsync_ReturnsFile()
        {
            // Arrange
            var fileId = 1; // Replace with a valid file ID
            var fileModel = new FileModel
            {
                Id = fileId,
                Name = "test.txt",
                Location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads/test.txt")
            };
            // Mock the necessary methods for file retrieval
            _unitOfWorkMock.Setup(repo => repo.FileRepository.GetFirstOrDefault(It.IsAny<Expression<Func<FileModel, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(fileModel);
            // Act
            // Set up the controller context with a mock http context
            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext { HttpContext = httpContext };
            _fileController.ControllerContext = controllerContext;
            var result = await _fileController.DownloadAsync(fileId);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsInstanceOf<APIResponse>(okResult.Value);
            var apiResponse = (APIResponse)okResult.Value;
            Assert.IsInstanceOf<Dictionary<string, byte[]>>(apiResponse.Result);
            var fileResult = (Dictionary<string, byte[]>)apiResponse.Result;
            // Verify that the file name and content are correct
            Assert.AreEqual(fileModel.Name, fileResult.Keys.First());
        }

        [Test]
        public async Task DownloadAsync_ReturnsNotFound()
        {
            // Arrange
            var fileId = 1; // Replace with a valid file ID
            var fileModel = new FileModel
            {
                Id = fileId,
                Name = "testNotFound.txt",
                Location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads/testNotFound.txt")
            };
            // Mock the necessary methods for file retrieval
            _unitOfWorkMock.Setup(repo => repo.FileRepository.GetFirstOrDefault(It.IsAny<Expression<Func<FileModel, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(fileModel);
            // Act
            // Set up the controller context with a mock http context           
            var result = await _fileController.DownloadAsync(fileId);
            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);            
        }

        [Test]
        public void Delete_ValidId_ReturnsOk()
        {
            // Arrange
            //create the file that we will delete
            IFormFile formFile = CreateMockFormFile("TestFileToDelete.txt");
            //string rootPath = _hostEnvironment.WebRootPath;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string uploadsPath = Path.Combine(baseDirectory, "uploads");
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(uploadsPath);
            using (var fileStreams = new FileStream(Path.Combine(uploadsPath, formFile.FileName), FileMode.Create))
            {
                formFile.CopyTo(fileStreams);
            }


            int fileId = 1;
            var fileModel = new FileModel
            {
                Id = fileId,
                Name = "TestFileToDelete.txt",
                Location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads/TestFileToDelete.txt")
            };
            _unitOfWorkMock.Setup(repo => repo.FileRepository.GetFirstOrDefault(It.IsAny<Expression<Func<FileModel, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
               .Returns(fileModel);
            // Act
            var result = _fileController.Delete(fileId);
            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.IsInstanceOf<APIResponse>(okResult.Value);
            var apiResponse = (APIResponse)okResult.Value;
            Assert.IsTrue(apiResponse.IsSuccess);
            Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            // Verify that Remove and Save methods were called
            _unitOfWorkMock.Verify(uow => uow.FileRepository.Remove(It.IsAny<FileModel>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Save(), Times.Once);
            // Verify that the file was deleted
            Assert.IsFalse(File.Exists(fileModel.Location));
        }

        [Test]
        public void Delete_FileDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            int fileId = 555; // Replace with a valid file ID
            _unitOfWorkMock.Setup(repo => repo.FileRepository.GetFirstOrDefault(It.IsAny<Expression<Func<FileModel, bool>>>(), It.IsAny<string>(), It.IsAny<bool>()))
               .Returns((FileModel) null);
            // Act
            var result = _fileController.Delete(fileId);
            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            
        }


        private IFormFile CreateMockFormFile(string fileName)
        {
            var content = "Mock file content";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var file = new FormFile(stream, 0, content.Length, "FormFile", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            };
            return file;
        }

    }

}
