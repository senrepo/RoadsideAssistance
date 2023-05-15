using Moq;
using RoadsideAssistanceBL.DataStore;
using RoadsideAssistanceBL.Models;
using RoadsideAssistanceBL.Service;
using System;
using System.Collections.Concurrent;

namespace RoadsideAssistanceBL.UnitTests
{
    public class RoadsideAssistanceServiceTests
    {
        private IRoadSideDataStore _dataStoreStub;
        private Mock<IAssistantService> _assistantSeviceMock;
        private Mock<ICustomerService> _customerSeviceMock;

        public RoadsideAssistanceServiceTests()
        {

        }

        [SetUp]
        public void Setup()
        {
            _dataStoreStub = new RoadSideDataStoreStub();
            _assistantSeviceMock = new Mock<IAssistantService>();
            _customerSeviceMock = new Mock<ICustomerService>();
        }

        [Test]
        public async Task TestFindNearestAssistants()
        {
            _assistantSeviceMock.Setup(x => x.GetAssistants()).Returns(async () =>
            {
                return await Task.Run(() =>
                {
                    return new ConcurrentBag<Assistant>(_dataStoreStub.GetAssitants());
                });

            });
            var limit = 3;
            var location = new Geolocation(2, 1);
            var roadsideAssistanceService = new RoadsideAssistanceService(_assistantSeviceMock.Object, _customerSeviceMock.Object);
            var response = await roadsideAssistanceService.FindNearestAssistants(location, limit);
            Assert.That(response.Count, Is.EqualTo(limit));
        }

        [Test]
        public async Task TestReserveAssistant()
        {
            _assistantSeviceMock.Setup(x => x.GetAssistants()).Returns(async () =>
            {
                return await Task.Run(() =>
                {
                    return new ConcurrentBag<Assistant>(_dataStoreStub.GetAssitants());
                });

            });

            var roadsideAssistanceService = new RoadsideAssistanceService(_assistantSeviceMock.Object, _customerSeviceMock.Object);
            var customer = new Customer()
            {
                Id = 1,
                Name = "Jack",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1001,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "xyz"
                    },
                    ProblemDescription = "flat tire"
                }
            };
            var location = new Geolocation(2, 1);
            var response = await roadsideAssistanceService.ReserveAssistant(customer, location);
            Assert.That(response, Is.TypeOf<Assistant>());
        }

        [Test]
        public async Task TestReleaseAssistant()
        {
            var roadsideAssistanceService = new RoadsideAssistanceService(_assistantSeviceMock.Object, _customerSeviceMock.Object);
            var customer = new Customer()
            {
                Id = 1,
                Name = "Jack",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1001,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "xyz"
                    },
                    ProblemDescription = "flat tire"
                }
            };
            var assistant = new Assistant
            {
                Id = 1,
                Name = "Rajesh",
            };
            await roadsideAssistanceService.ReleaseAssistant(customer, assistant);
            Assert.Pass(); //no error means pass
        }

        [Test]
        public async Task TestUpdateAssistantLocation()
        {
            var roadsideAssistanceService = new RoadsideAssistanceService(_assistantSeviceMock.Object, _customerSeviceMock.Object);
            var assistant = new Assistant
            {
                Id = 1,
                Name = "Rajesh",
            };
            var location = new Geolocation(2, 1);
            await roadsideAssistanceService.UpdateAssistantLocation(assistant, location);
            Assert.Pass(); //no error means pass
        }
    }
}