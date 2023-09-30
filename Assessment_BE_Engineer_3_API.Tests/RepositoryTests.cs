using Assessment_BE_Engineer_3_API.Data;
using Assessment_BE_Engineer_3_API.Models;
using Assessment_BE_Engineer_3_API.Repository;
using Assessment_BE_Engineer_3_API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq.Expressions;

namespace Assessment_BE_Engineer_3_API.Tests
{
    public class RepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> _options;
        ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(_options);
        }
        [Test]
        public async Task AddAsync_ShouldAddEntityToInMemoryDatabase()
        {
            // Arrange
            var entity = new FileModel()
            {
                Name = "test.txt",
                Size = "15 KB",
                Location = "/testlocation"
            };
            var _repository = new Repository<FileModel>(_context);
            // Act
            _repository.AddAsync(entity);
            _context.SaveChanges(); // Ensure changes are saved

            // Assert
            using (var context = new ApplicationDbContext(_options))
            {
                var result = _context.Set<FileModel>().FirstOrDefault(e => e.Id == entity.Id);
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public async Task GetAllAsync_WithFilter_ShouldReturnFilteredEntities()
        {
            // Arrange
            var entity1 = new FileModel()
            {
                Name = "test1.txt",
                Size = "15 KB",
                Location = "/testlocation"
            };
            var entity2 = new FileModel()
            {
                Name = "test2.txt",
                Size = "25 KB",
                Location = "/testlocation"
            };
            //make sure db is empty
            _context.Database.EnsureDeleted();
            var _repository = new Repository<FileModel>(_context);
            _repository.AddAsync(entity1);
            _repository.AddAsync(entity2);
            _context.SaveChanges();

            // Act
            Expression<Func<FileModel, bool>> filter = e => e.Id == entity1.Id;

            var result = await _repository.GetAllAsync(filter, "");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(entity1.Id, result.First().Id);
            // Add more assertions based on your requirements
        }

        [Test]
        public async Task GetAllAsync_WithoutFilter_ShouldReturnAllEntities()
        {
            // Arrange
            var entity1 = new FileModel()
            {
                Name = "test1.txt",
                Size = "15 KB",
                Location = "/testlocation"
            };
            var entity2 = new FileModel()
            {
                Name = "test2.txt",
                Size = "25 KB",
                Location = "/testlocation"
            };
            //make sure db is empty
            _context.Database.EnsureDeleted();
            var _repository = new Repository<FileModel>(_context);
            _repository.AddAsync(entity1);
            _repository.AddAsync(entity2);
            _context.SaveChanges();

            // Act
            var result = await _repository.GetAllAsync();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            // Add more assertions based on your requirements
        }

        [Test]
        public async Task GetFirstOrDefault_WithFilter_ShouldReturnFilteredEntity()
        {
            /// Arrange
            var entity1 = new FileModel()
            {
                Name = "test1.txt",
                Size = "15 KB",
                Location = "/testlocation"
            };
            var entity2 = new FileModel()
            {
                Name = "test2.txt",
                Size = "25 KB",
                Location = "/testlocation"
            };
            //make sure db is empty
            _context.Database.EnsureDeleted();
            var _repository = new Repository<FileModel>(_context);
            _repository.AddAsync(entity1);
            _repository.AddAsync(entity2);
            _context.SaveChanges();

            // Act
            Expression<Func<FileModel, bool>> filter = e => e.Id == entity1.Id;
            var result = _repository.GetFirstOrDefault(filter, "");
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(entity1.Id, result.Id);
        }

        [Test]
        public async Task Remove_WithValidEntity_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            var entityToRemove = new FileModel()
            {
                Name = "test1.txt",
                Size = "15 KB",
                Location = "/testlocation"
            };

            //make sure db is empty
            _context.Database.EnsureDeleted();
            var _repository = new Repository<FileModel>(_context);
            _repository.AddAsync(entityToRemove);
            _context.SaveChanges();

            // Act
            _repository.Remove(entityToRemove);
            _context.SaveChanges();

            // Assert
            var result = _repository.GetFirstOrDefault(e => e.Id == entityToRemove.Id);

            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoveRange_WithValidEntity_ShouldRemoveEntityFromDatabase()
        {
            // Arrange
            var entity1 = new FileModel()
            {
                Name = "test1.txt",
                Size = "15 KB",
                Location = "/testlocation"
            };
            var entity2 = new FileModel()
            {
                Name = "test2.txt",
                Size = "25 KB",
                Location = "/testlocation"
            };
            List<FileModel> entitiesToRemove = new List<FileModel>();
            entitiesToRemove.Add(entity1);
            entitiesToRemove.Add(entity2);
            //make sure db is empty
            _context.Database.EnsureDeleted();
            var _repository = new Repository<FileModel>(_context);
            _repository.AddAsync(entity1);
            _repository.AddAsync(entity2);
            _context.SaveChanges();

            // Act
            _repository.RemoveRange(entitiesToRemove);
            _context.SaveChanges();

            // Assert
            var result = await _repository.GetAllAsync();

            Assert.That(result, Is.Empty);
        }
    }
}