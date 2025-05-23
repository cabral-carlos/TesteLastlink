using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using EnterpriseAPI;
using EnterpriseAPI.Models;
using EnterpriseAPI.Repositories;

namespace Enterprise.MSTests
{
    [TestClass]
    public class CreatorsRepositoryTests
    {
        private DbContextOptions<AppDBContext> _dbContextOptions;

        [TestInitialize]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "enterpriseTest")
                .Options;
        }


        [TestMethod]
        public void AddCreator_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeCreator = new Creator
            {
                Id = 10,
                Name = "John",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            // Act
            using (var context = new AppDBContext(_dbContextOptions))
            {
                var repo = new CreatorsRepository(context);
                repo.Add(fakeCreator).Wait();
            }

            // Assert
            using (var context = new AppDBContext(_dbContextOptions))
            {
                var creatorFromDB = context.Creators.ToList().Where(x => x.Id == 10).FirstOrDefault();
                Assert.AreEqual(10, creatorFromDB.Id);
                Assert.AreEqual(fakeCreator.Name, creatorFromDB.Name);
                Assert.AreEqual(fakeCreator.IsActive, creatorFromDB.IsActive);
            }
        }

        [TestMethod]
        public void DeleteCreator_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeCreator = new Creator
            {
                Id = 1,
                Name = "John",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            
            using var context = new AppDBContext(_dbContextOptions);
            var repo = new CreatorsRepository(context);

            // Act
            repo.Delete(fakeCreator).Wait();

            // Assert
            var creatorFromDB = repo.GetByID(1);
            Assert.IsNull(creatorFromDB);
        }

        [TestMethod]
        public void GetCreators_NoCreatorExists_ResultIsNull()
        {
            // Arrange
            using var context = new AppDBContext(new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "enterpriseEmptyTest")
                .Options);
            var repo = new CreatorsRepository(context);

            // Act
            var creatorsFromDB = repo.Get();

            // Assert
            Assert.IsFalse(creatorsFromDB.Any());
        }

        [TestMethod]
        public void GetCreators_ThereAreCreatorsInDatabase_ResultIsSuccess()
        {
            // Arrange
            using var context = new AppDBContext(_dbContextOptions);
            var repo = new CreatorsRepository(context);

            // Act
            var creatorsFromDB = repo.Get();

            // Assert
            Assert.IsTrue(creatorsFromDB.Any());
        }

        [TestMethod]
        public void GetCreatorById_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeCreator = new Creator
            {
                Id = 5,
                Name = "John",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            using var context = new AppDBContext(_dbContextOptions);
            var repo = new CreatorsRepository(context);
            repo.Add(fakeCreator).Wait();

            // Act
            var creatorFromDB = repo.GetByID(5);

            // Assert
            Assert.IsNotNull(creatorFromDB);
            Assert.AreEqual(5, creatorFromDB.Id);
            Assert.AreEqual(fakeCreator.Name, creatorFromDB.Name);
            Assert.AreEqual(fakeCreator.IsActive, creatorFromDB.IsActive);
        }

        [TestMethod]
        public void GetCreatorById_IdWasNotFound_ResultIsNull()
        {
            // Arrange
            using var context = new AppDBContext(_dbContextOptions);
            var repo = new CreatorsRepository(context);

            // Act
            var creatorFromDB = repo.GetByID(99);

            // Assert
            Assert.IsNull(creatorFromDB);
        }
    }
}
