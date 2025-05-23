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
    public class RequestsBusinessTests
    {
        private Mock<IRequestsRepository> _mockRequestsRepository;
        private Mock<ICreatorsRepository> _mockCreatorsRepository;
        private RequestsBusiness _mockRequestsBusiness;

        [TestInitialize]
        public void Setup()
        {
            _mockRequestsRepository = new Mock<IRequestsRepository>();
            _mockCreatorsRepository = new Mock<ICreatorsRepository>();
            _mockRequestsBusiness = new RequestsBusiness(_mockRequestsRepository.Object, _mockCreatorsRepository.Object);
        }

        [TestMethod]
        public async Task ApproveRequest_RequestWasNotFound_ResultIsFalse()
        {
            // Arrange
            _mockRequestsRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns((Request)null);

            // Act
            var result = await _mockRequestsBusiness.ApproveRequest(1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ApproveRequest_RequestWasNotPending_ResultIsFalse()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatorId = 15,
                Status = Status.Approved
            };
            _mockRequestsRepository.Setup(r => r.GetById(fakeRequest.Id)).Returns(fakeRequest);

            // Act
            var result = await _mockRequestsBusiness.ApproveRequest(fakeRequest.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ApproveRequest_ProcessGotAnError_ResultIsFalse()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatorId = 15
            };
            _mockRequestsRepository.Setup(r => r.GetById(fakeRequest.Id)).Returns(fakeRequest);
            _mockRequestsRepository.Setup(r => r.Approve(fakeRequest)).ReturnsAsync(false);

            // Act
            var result = await _mockRequestsBusiness.ApproveRequest(fakeRequest.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task ApproveRequest_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatorId = 15
            };
            _mockRequestsRepository.Setup(r => r.GetById(fakeRequest.Id)).Returns(fakeRequest);
            _mockRequestsRepository.Setup(r => r.Approve(fakeRequest)).ReturnsAsync(true);

            // Act
            var result = await _mockRequestsBusiness.ApproveRequest(fakeRequest.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CreateRequest_CreatorIdWasNotFound_ResultIsFalse()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                CreatorId = 15
            };
            _mockCreatorsRepository.Setup(r => r.GetById(fakeRequest.CreatorId)).Returns((Creator)null);

            // Act
            var result = await _mockRequestsBusiness.CreateRequest(fakeRequest);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateRequest_CreatorHasMoreThanOnePendingRequest_ResultIsFalse()
        {
            // Arrange
            var fakeCreator = new Creator
            {
                Id = 15,
                Name = "Mark",
                CreatedAt = DateTime.Now,
                IsActive = true
            };
            
            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                CreatorId = fakeCreator.Id
            };

            _mockCreatorsRepository.Setup(r => r.GetById(fakeRequest.CreatorId)).Returns(fakeCreator);
            _mockRequestsRepository.Setup(r => r.CountPendingByCreatorId(fakeRequest.CreatorId)).Returns(3);

            // Act
            var result = await _mockRequestsBusiness.CreateRequest(fakeRequest);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateRequest_RequestGrossValueIsNotAllowed_ResultIsFalse()
        {
            // Arrange
            var fakeCreator = new Creator
            {
                Id = 15,
                Name = "Mark",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 2,
                CreatorId = fakeCreator.Id
            };

            _mockCreatorsRepository.Setup(r => r.GetById(fakeRequest.CreatorId)).Returns(fakeCreator);
            _mockRequestsRepository.Setup(r => r.CountPendingByCreatorId(fakeRequest.CreatorId)).Returns(0);

            // Act
            var result = await _mockRequestsBusiness.CreateRequest(fakeRequest);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateRequest_ProcessGotAnError_ResultIsFalse()
        {
            // Arrange
            var fakeCreator = new Creator
            {
                Id = 15,
                Name = "Mark",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 1000,
                CreatorId = fakeCreator.Id
            };

            _mockCreatorsRepository.Setup(r => r.GetById(fakeRequest.CreatorId)).Returns(fakeCreator);
            _mockRequestsRepository.Setup(r => r.CountPendingByCreatorId(fakeRequest.CreatorId)).Returns(0);
            _mockRequestsRepository.Setup(r => r.Create(fakeRequest)).ReturnsAsync(false);

            // Act
            var result = await _mockRequestsBusiness.CreateRequest(fakeRequest);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task CreateRequest_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeCreator = new Creator
            {
                Id = 15,
                Name = "Mark",
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 1000,
                CreatorId = fakeCreator.Id
            };

            _mockCreatorsRepository.Setup(r => r.GetById(fakeRequest.CreatorId)).Returns(fakeCreator);
            _mockRequestsRepository.Setup(r => r.CountPendingByCreatorId(fakeRequest.CreatorId)).Returns(0);
            _mockRequestsRepository.Setup(r => r.Create(fakeRequest)).ReturnsAsync(true);

            // Act
            var result = await _mockRequestsBusiness.CreateRequest(fakeRequest);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task DenyRequest_RequestWasNotFound_ResultIsFalse()
        {
            // Arrange
            _mockRequestsRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns((Request)null);

            // Act
            var result = await _mockRequestsBusiness.DenyRequest(1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DenyRequest_RequestWasNotPending_ResultIsFalse()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatorId = 15,
                Status = Status.Approved
            };
            _mockRequestsRepository.Setup(r => r.GetById(fakeRequest.Id)).Returns(fakeRequest);

            // Act
            var result = await _mockRequestsBusiness.DenyRequest(fakeRequest.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DenyRequest_ProcessGotAnError_ResultIsFalse()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatorId = 15
            };
            _mockRequestsRepository.Setup(r => r.GetById(fakeRequest.Id)).Returns(fakeRequest);
            _mockRequestsRepository.Setup(r => r.Deny(fakeRequest)).ReturnsAsync(false);

            // Act
            var result = await _mockRequestsBusiness.DenyRequest(fakeRequest.Id);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DenyRequest_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 15,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatorId = 15
            };
            _mockRequestsRepository.Setup(r => r.GetById(fakeRequest.Id)).Returns(fakeRequest);
            _mockRequestsRepository.Setup(r => r.Deny(fakeRequest)).ReturnsAsync(true);

            // Act
            var result = await _mockRequestsBusiness.DenyRequest(fakeRequest.Id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetRequestByCreatorId_ParametersAreCorrect_ResultIsSuccess()
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
            _mockRequestsRepository.Setup(r => r.GetByCreatorId(3)).Returns(fakeRequests);

            // Act
            var requestsFromDB = _mockRequestsBusiness.GetRequestsByCreatorId(3);

            // Assert
            Assert.AreEqual(2, requestsFromDB.Count());
        }

        [TestMethod]
        public void GetRequestByCreatorId_CreatorIdWasNotFound_ResultIsZero()
        {
            // Arrange
            var emptylist = new List<Request>();
            _mockRequestsRepository.Setup(r => r.GetByCreatorId(It.IsAny<int>())).Returns(emptylist);

            // Act
            var requestsFromDB = _mockRequestsBusiness.GetRequestsByCreatorId(1);

            // Assert
            Assert.IsFalse(requestsFromDB.Any());
        }

        [TestMethod]
        public void GetRequestById_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 7,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatorId = 3
            };

            _mockRequestsRepository.Setup(r => r.GetById(7)).Returns(fakeRequest);

            // Act
            var requestFromDB = _mockRequestsBusiness.GetById(7);

            // Assert
            Assert.IsNotNull(requestFromDB);
            Assert.AreEqual(7, requestFromDB.Id);
            Assert.AreEqual(fakeRequest.GrossValue, requestFromDB.GrossValue);
            Assert.AreEqual(fakeRequest.Type, requestFromDB.Type);
            Assert.AreEqual(fakeRequest.Fee, requestFromDB.Fee);
            Assert.AreEqual(fakeRequest.NetValue, requestFromDB.NetValue);
            Assert.AreEqual(fakeRequest.CreatorId, requestFromDB.CreatorId);
            Assert.AreEqual(Status.Pending, requestFromDB.Status);
        }

        [TestMethod]
        public void GetRequestById_IdWasNotFound_ResultIsNull()
        {
            // Arrange
            _mockRequestsRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns((Request)null);

            // Act
            var requestFromDB = _mockRequestsBusiness.GetById(99);

            // Assert
            Assert.IsNull(requestFromDB);
        }
    }
}
