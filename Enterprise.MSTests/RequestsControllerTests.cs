using EnterpriseAPI.Business;
using EnterpriseAPI.Controllers;
using EnterpriseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.MSTests
{
    [TestClass]
    public class RequestsControllerTests
    {
        private Mock<IRequestsBusiness> _mockRequestsBusiness;
        private RequestsController _requestsController;

        [TestInitialize]
        public void Setup()
        {
            _mockRequestsBusiness = new Mock<IRequestsBusiness>();
            _requestsController = new RequestsController(_mockRequestsBusiness.Object);
        }

        [TestMethod]
        public void AddRequest_RequestWasNotFound_ResultIsBadRequest()
        {
            // Arrange and Act
            var result = _requestsController.AddRequest(null).Result;

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Required parameters are missing", badRequestResult.Value);
        }

        [TestMethod]
        public void AddRequest_DateFormatIsInvalid_ResultIsBadRequest()
        {
            // Arrange
            var fakeParameters = new RequestParameters
            {
                CreatorId = 1,
                Date = "",
                Type = 1,
                Value = 200
            };

            // Act
            var result = _requestsController.AddRequest(fakeParameters).Result;

            // Assert
            var objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual("Invalid date format", objectResult.Value);
        }

        [TestMethod]
        public void AddRequest_ProcessGotAnError_ResultIsInternalServerError()
        {
            // Arrange
            var fakeParameters = new RequestParameters
            {
                CreatorId = 1,
                Date = "22/05/2025",
                Type = 1,
                Value = 200
            };

            DateTime.TryParseExact(fakeParameters.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime formattedDate);

            var fakeRequest = new Request()
            {
                CreatorId = fakeParameters.CreatorId,
                Date = formattedDate,
                Type = (EnterpriseAPI.Models.Type)fakeParameters.Type,
                GrossValue = fakeParameters.Value
            };

            _mockRequestsBusiness.Setup(b => b.CreateRequest(fakeRequest)).ReturnsAsync(false);

            // Act
            var result = _requestsController.AddRequest(fakeParameters).Result;

            // Assert
            var objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unexpected error", objectResult.Value);
        }

        [TestMethod]
        public void AddRequest_ParametersAreCorrect_ResultIsCreated()
        {
            // Arrange
            var fakeParameters = new RequestParameters
            {
                CreatorId = 1,
                Date = "22/05/2025",
                Type = 1,
                Value = 200
            };

            _mockRequestsBusiness.Setup(b => b.CreateRequest(It.IsAny<Request>())).ReturnsAsync(true);

            // Act
            var result = _requestsController.AddRequest(fakeParameters).Result;

            // Assert
            var createdResult = (StatusCodeResult)result;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
        }

        [TestMethod]
        public void ApproveRequest_RequestWasNotFound_ResultIsBadRequest()
        {
            // Arrange and Act
            var result = _requestsController.Approve(-1).Result;

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("The parameter should be greater than zero", badRequestResult.Value);
        }

        [TestMethod]
        public void ApproveRequest_ProcessGotAnError_ResultIsInternalServerError()
        {
            // Arrange
            _mockRequestsBusiness.Setup(b => b.ApproveRequest(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = _requestsController.Approve(1).Result;

            // Assert
            var objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unexpected error", objectResult.Value);
        }

        [TestMethod]
        public void ApproveRequest_ParametersAreCorrect_ResultIsOk()
        {
            // Arrange
            _mockRequestsBusiness.Setup(b => b.ApproveRequest(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = _requestsController.Approve(1).Result;

            // Assert
            var okResult = (OkResult)result;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void DenyRequest_RequestWasNotFound_ResultIsBadRequest()
        {
            // Arrange and Act
            var result = _requestsController.Deny(-1).Result;

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("The parameter should be greater than zero", badRequestResult.Value);
        }

        [TestMethod]
        public void DenyRequest_ProcessGotAnError_ResultIsInternalServerError()
        {
            // Arrange
            _mockRequestsBusiness.Setup(b => b.DenyRequest(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = _requestsController.Deny(1).Result;

            // Assert
            var objectResult = (ObjectResult)result;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Unexpected error", objectResult.Value);
        }

        [TestMethod]
        public void DenyRequest_ParametersAreCorrect_ResultIsOk()
        {
            // Arrange
            _mockRequestsBusiness.Setup(b => b.DenyRequest(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = _requestsController.Deny(1).Result;

            // Assert
            var okResult = (OkResult)result;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void GetAllByCreatorId_CreatorIdIsInvalid_ResultIsBadRequest()
        {
            // Arrange and Act
            var result = _requestsController.GetAllByCreatorId(-1);

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("The parameter should be greater than zero", badRequestResult.Value);
        }

        [TestMethod]
        public void GetAllByCreatorId_ThereAreNoRequests_ResultIsNotFound()
        {
            // Arrange
            _mockRequestsBusiness.Setup(b => b.GetRequestsByCreatorId(It.IsAny<int>())).Returns((IEnumerable<Request>)null);

            // Act
            var result = _requestsController.GetAllByCreatorId(1);

            // Assert
            var noContentResult = (NotFoundResult)result;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(404, noContentResult.StatusCode);
        }

        [TestMethod]
        public void GetAllByCreatorId_ParametersAreCorrect_ResultIsOk()
        {
            // Arrange
            var fakeRequests = new List<Request>()
            {
                new Request
                {
                    Type = EnterpriseAPI.Models.Type.Anticipation,
                    Date = DateTime.Now,
                    GrossValue = 100,
                    Fee = 5,
                    NetValue = 95,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatorId = 3
                },
                new Request
                {
                    Type = EnterpriseAPI.Models.Type.Anticipation,
                    Date = DateTime.Now,
                    GrossValue = 100,
                    Fee = 50,
                    NetValue = 50,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatorId = 3,
                    Status = Status.Denied
                }
            };

            _mockRequestsBusiness.Setup(b => b.GetRequestsByCreatorId(3)).Returns(fakeRequests);

            // Act
            var result = _requestsController.GetAllByCreatorId(3);

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(fakeRequests, okResult.Value);
        }
    }
}
