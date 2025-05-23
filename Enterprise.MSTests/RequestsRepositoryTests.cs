using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using EnterpriseAPI;
using EnterpriseAPI.Models;
using EnterpriseAPI.Repositories;

namespace Enterprise.MSTests
{
    [TestClass]
    public class RequestsRepositoryTests
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
        public void ApproveRequests_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 12,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatorId = 12
            };

            // Act and Assert
            using (var context = new AppDBContext(_dbContextOptions))
            {
                var repo = new RequestsRepository(context);
                repo.Create(fakeRequest).Wait();

                var requestFromDB = repo.GetById(12);
                Assert.AreEqual(Status.Pending, requestFromDB.Status);

                repo.Approve(fakeRequest).Wait();
            
                var approvedFakeRequest = repo.GetById(12);
                Assert.AreEqual(Status.Approved, approvedFakeRequest.Status);
            }
        }

        [TestMethod]
        public void CountPendingByCreatorId_CreatorIdWasNotFound_ResultIsZero()
        {
            // Arrange
            using var context = new AppDBContext(_dbContextOptions);
            var repo = new RequestsRepository(context);

            // Act
            var requestsFromDB = repo.CountPendingByCreatorId(99);

            // Assert
            Assert.AreEqual(0, requestsFromDB);
        }

        [TestMethod]
        public void CountPendingByCreatorId_CreatorHasManyRequests_ResultIsOnlyPendingReturns()
        {
            // Arrange
            List<Request> fakeRequests = new List<Request>()
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
                    CreatorId = 5
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
                    CreatorId = 5,
                    Status = Status.Denied
                }
            };

            using var context = new AppDBContext(_dbContextOptions);
            var repo = new RequestsRepository(context);
            repo.Create(fakeRequests[0]).Wait();
            repo.Create(fakeRequests[1]).Wait();

            // Act
            var requestsFromDB = repo.CountPendingByCreatorId(5);

            // Assert
            Assert.AreNotEqual(fakeRequests.Count, requestsFromDB);
        }

        [TestMethod]
        public void CreateRequests_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var fakeRequest = new Request
            {
                Id = 10,
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatorId = 1
            };

            // Act
            using (var context = new AppDBContext(_dbContextOptions))
            {
                var repo = new RequestsRepository(context);
                repo.Create(fakeRequest).Wait();
            }

            // Assert
            using (var context = new AppDBContext(_dbContextOptions))
            {
                var requestFromDB = context.Requests.ToList().Where(x => x.CreatorId == 1).FirstOrDefault();
                Assert.AreEqual(10, requestFromDB.Id);
                Assert.AreEqual(fakeRequest.GrossValue, requestFromDB.GrossValue);
                Assert.AreEqual(fakeRequest.Type, requestFromDB.Type);
                Assert.AreEqual(fakeRequest.Fee, requestFromDB.Fee);
                Assert.AreEqual(fakeRequest.NetValue, requestFromDB.NetValue);
                Assert.AreEqual(fakeRequest.CreatorId, requestFromDB.CreatorId);
                Assert.AreEqual(Status.Pending, requestFromDB.Status);
            }
        }

        [TestMethod]
        public void DenyRequests_ParametersAreCorrect_ResultIsSuccess()
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

            // Act and Assert
            using (var context = new AppDBContext(_dbContextOptions))
            {
                var repo = new RequestsRepository(context);
                repo.Create(fakeRequest).Wait();

                var requestFromDB = repo.GetById(15);
                Assert.AreEqual(Status.Pending, requestFromDB.Status);

                repo.Deny(fakeRequest).Wait();

                var approvedFakeRequest = repo.GetById(15);
                Assert.AreEqual(Status.Denied, approvedFakeRequest.Status);
            }
        }

        [TestMethod]
        public void GetRequestByCreatorId_ParametersAreCorrect_ResultIsSuccess()
        {
            // Arrange
            var firstFakeRequest = new Request
            {
                Type = EnterpriseAPI.Models.Type.Anticipation,
                Date = DateTime.Now,
                GrossValue = 100,
                Fee = 5,
                NetValue = 95,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CreatorId = 3
            };

            var secondFakeRequest = new Request
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
            };

            using var context = new AppDBContext(_dbContextOptions);
            var creatorsRepo = new CreatorsRepository(context);
            creatorsRepo.Add(new Creator()
            {
                Id = 3,
                Name = "Mark",
                CreatedAt = DateTime.Now,
                IsActive = true
            }).Wait();

            var requestsRepo = new RequestsRepository(context);
            requestsRepo.Create(firstFakeRequest).Wait();
            requestsRepo.Create(secondFakeRequest).Wait();

            // Act
            var requestsFromDB = requestsRepo.GetByCreatorId(3);

            // Assert
            Assert.AreEqual(2, requestsFromDB.Count());
        }

        [TestMethod]
        public void GetRequestByCreatorId_CreatorIdWasNotFound_ResultIsZero()
        {
            // Arrange
            using var context = new AppDBContext(_dbContextOptions);
            var repo = new RequestsRepository(context);

            // Act
            var requestsFromDB = repo.GetByCreatorId(99);

            // Assert
            Assert.AreEqual(0, requestsFromDB.Count());
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

            using var context = new AppDBContext(_dbContextOptions);
            var repo = new RequestsRepository(context);
            repo.Create(fakeRequest).Wait();

            // Act
            var requestFromDB = repo.GetById(7);

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
            using var context = new AppDBContext(_dbContextOptions);
            var repo = new RequestsRepository(context);

            // Act
            var requestFromDB = repo.GetById(99);

            // Assert
            Assert.IsNull(requestFromDB);
        }
    }
}
