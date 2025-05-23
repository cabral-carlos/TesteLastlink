using EnterpriseAPI.Business;
using EnterpriseAPI.Controllers;
using EnterpriseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.MSTests
{
    [TestClass]
    public class CreatorsControllerTests
    {
        private Mock<ICreatorsBusiness> _mockCreatorsBusiness;
        private CreatorsController _creatorsController;

        [TestInitialize]
        public void Setup()
        {
            _mockCreatorsBusiness = new Mock<ICreatorsBusiness>();
            _creatorsController = new CreatorsController(_mockCreatorsBusiness.Object);
        }

        [TestMethod]
        public void AddCreator_CreatorNameIsNotValid_ResultIsBadRequest()
        {
            // Arrange and Act
            var result = _creatorsController.AddCreator("").Result;

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("The parameter should not be empty", badRequestResult.Value);
        }

        [TestMethod]
        public void AddCreator_ProcessGotAnError_ResultIsInternalServerError()
        {
            // Arrange
            _mockCreatorsBusiness.Setup(b => b.AddCreator(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = _creatorsController.AddCreator("Mark").Result;

            // Assert
            var objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unexpected error", objectResult.Value);
        }

        [TestMethod]
        public void AddCreator_ParametersAreCorrect_ResultIsCreated()
        {
            // Arrange
            var name = "Mark";
            _mockCreatorsBusiness.Setup(b => b.AddCreator(name)).ReturnsAsync(true);

            // Act
            var result = _creatorsController.AddCreator(name).Result;

            // Assert
            var createdResult = (StatusCodeResult)result;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [TestMethod]
        public void DeleteCreator_CreatorIdIsNotValid_ResultIsBadRequest()
        {
            // Arrange and Act
            var result = _creatorsController.Delete(-1).Result;

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("The parameter should be greater than zero", badRequestResult.Value);
        }

        [TestMethod]
        public void DeleteCreator_CreatorIdWasNotFound_ResultIsInternalServerError()
        {
            // Arrange
            _mockCreatorsBusiness.Setup(b => b.DeleteCreator(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = _creatorsController.Delete(1).Result;

            // Assert
            var objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unexpected error", objectResult.Value);
        }

        [TestMethod]
        public void DeleteCreator_CreatorIdWasFound_ResultIsOk()
        {
            // Arrange
            var fakeCreator = new Creator()
            {
                Id = 1,
                Name = "Mark",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            _mockCreatorsBusiness.Setup(b => b.DeleteCreator(fakeCreator.Id)).ReturnsAsync(true);

            // Act
            var result = _creatorsController.Delete(fakeCreator.Id).Result;

            // Assert
            var okResult = (OkResult)result;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void GetAll_ThereAreNoCreators_ResultIsNoContent()
        {
            // Arrange
            _mockCreatorsBusiness.Setup(b => b.GetAll()).Returns((IEnumerable<Creator>)null);

            // Act
            var result = _creatorsController.Get();

            // Assert
            var noContentResult = (NoContentResult)result;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [TestMethod]
        public void GetAll_ParametersAreCorrect_ResultIsOk()
        {
            // Arrange
            var fakeCreators = new List<Creator>() 
            {
                new Creator 
                {
                    Id = 1,
                    Name = "Mark",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new Creator
                {
                    Id = 2,
                    Name = "John",
                    CreatedAt = DateTime.Now,
                    IsActive = false
                }
            };
            _mockCreatorsBusiness.Setup(b => b.GetAll()).Returns(fakeCreators);

            // Act
            var result = _creatorsController.Get();

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(fakeCreators, okResult.Value);
        }

        [TestMethod]
        public void GetById_CreatorIdIsInvalid_ResultIsBadRequest()
        {
            // Arrange and Act
            var result = _creatorsController.Get(-1);

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("The parameter should be greater than zero", badRequestResult.Value);
        }

        [TestMethod]
        public void GetById_CreatorIdWasNotFound_ResultIsNotFound()
        {
            // Arrange
            _mockCreatorsBusiness.Setup(b => b.GetCreatorById(It.IsAny<int>())).Returns((Creator)null);

            // Act
            var result = _creatorsController.Get(1);

            // Assert
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Creator was not found", notFoundResult.Value);
        }

        [TestMethod]
        public void GetById_CreatorIdWasFound_ResultIsOk()
        {
            // Arrange
            var fakeCreator = new Creator()
            {
                Id = 1,
                Name = "Mark",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            _mockCreatorsBusiness.Setup(b => b.GetCreatorById(fakeCreator.Id)).Returns(fakeCreator);

            // Act
            var result = _creatorsController.Get(fakeCreator.Id);

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(fakeCreator, okResult.Value);
        }
    }
}
