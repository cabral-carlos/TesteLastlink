using EnterpriseAPI.Business;
using EnterpriseAPI.Models;
using EnterpriseAPI.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise.MSTests
{
    [TestClass]
    public class CreatorsBusinessTests
    {
        private Mock<ICreatorsRepository> _mockCreatorsRepository;
        private CreatorsBusiness _mockCreatorsBusiness;

        [TestInitialize]
        public void Setup()
        {
            _mockCreatorsRepository = new Mock<ICreatorsRepository>();
            _mockCreatorsBusiness = new CreatorsBusiness(_mockCreatorsRepository.Object);
        }

        [TestMethod]
        public async Task AddCreator_ProcessGotAnError_ResultIsFalse()
        {
            // Arrange
            _mockCreatorsRepository.Setup(r => r.Add(It.IsAny<Creator>())).ReturnsAsync(false);

            // Act
            var result = await _mockCreatorsBusiness.AddCreator("John");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task AddCreator_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            _mockCreatorsRepository.Setup(r => r.Add(It.IsAny<Creator>())).ReturnsAsync(true);

            // Act
            var result = await _mockCreatorsBusiness.AddCreator("John");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DeleteCreator_CreatorWasNotFound_ResultIsFalse()
        {
            // Arrange
            _mockCreatorsRepository.Setup(r => r.Delete(It.IsAny<Creator>())).ReturnsAsync(true);

            // Act
            var result = await _mockCreatorsBusiness.DeleteCreator(1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteCreator_CreatorWasFound_ResultIsTrue()
        {
            // Arrange
            var fakeCreator = new Creator
            {
                Id = 1,
                Name = "John",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _mockCreatorsRepository.Setup(r => r.GetById(fakeCreator.Id)).Returns(fakeCreator);
            _mockCreatorsRepository.Setup(r => r.Delete(fakeCreator)).ReturnsAsync(true);

            // Act
            var result = await _mockCreatorsBusiness.DeleteCreator(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetCreators_NoCreatorExists_ResultIsNull()
        {
            // Arrange
            var emptyList = new List<Creator>();
            _mockCreatorsRepository.Setup(r => r.Get()).Returns(emptyList);

            // Act
            var creatorsFromDB = _mockCreatorsBusiness.GetAll();

            // Assert
            Assert.IsFalse(creatorsFromDB.Any());
        }

        [TestMethod]
        public void GetCreators_ThereAreCreatorsInDatabase_ResultIsSuccess()
        {
            // Arrange
            var fakeCreators = new List<Creator>()
            {
                new Creator
                {
                    Id = 5,
                    Name = "John",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }
            };
            _mockCreatorsRepository.Setup(r => r.Get()).Returns(fakeCreators);

            // Act
            var creatorsFromDB = _mockCreatorsBusiness.GetAll();

            // Assert
            Assert.IsTrue(creatorsFromDB.Any());
        }

        [TestMethod]
        public void GetCreatorById_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var creatorId = 5;
            var fakeCreator = new Creator
            {
                Id = creatorId,
                Name = "John",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _mockCreatorsRepository.Setup(r => r.GetById(creatorId)).Returns(fakeCreator);

            // Act
            var creatorFromDB = _mockCreatorsBusiness.GetCreatorById(creatorId);

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
            _mockCreatorsRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns((Creator)null);

            // Act
            var creatorFromDB = _mockCreatorsBusiness.GetCreatorById(1);

            // Assert
            Assert.IsNull(creatorFromDB);
        }
    }
}
