using RoadsideAssistanceBL.DataStore;
using RoadsideAssistanceBL.Models;
using RoadsideAssistanceBL.Service;

namespace RoadsideAssistanceBL.IntegrationTests
{
    public class RoadsideAssistanceMainFlowTests
    {
        private IRoadSideDataStore _dataStoreStub;
        private IAssistantService _assistantSevice;
        private ICustomerService _customerSevice;

        public RoadsideAssistanceMainFlowTests()
        {
            _dataStoreStub = new RoadSideDataStoreStub();
        }

        [SetUp]
        public void Setup()
        {
            _assistantSevice = new AssistantService(_dataStoreStub);
            _customerSevice = new CustomerService(_dataStoreStub);
        }

        [Test]
        public async Task MainFlowTests()
        {
            /*
                * Main flow
                * Step 1: Check if any available assistants near by where an assistance needed
                * Step 2: Customer creates a roadside assitance service request
                * Step 3: resserve an assitant
                * Step 4: Once the job done, release the assitant
                * Step 5: assistant update his location
            */
            var location = new Geolocation(2, 1);
            var limit = 3;

            var roadsideAssistanceService = new RoadsideAssistanceService(_assistantSevice, _customerSevice);

            //Step 1: Customer check if any assistants near by where an assistance needed
            var nearbyAssitants = await roadsideAssistanceService.FindNearestAssistants(location, limit);
            Assert.That(nearbyAssitants.Count, Is.GreaterThan(0));

            //Step 2: Customer creates a roadside assitance service request
            var customer = new Customer()
            {
                Id = 1,
                Name = "Jack",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1001,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "AC67035"
                    },
                    ProblemDescription = "flat tire"
                }
            };

            //Step 3: resserve an assitant
            var assistant = await roadsideAssistanceService.ReserveAssistant(customer, location);
            Assert.That(assistant, Is.TypeOf<Assistant>());

            //Step 4: Once the job done, release the assitant
            await roadsideAssistanceService.ReleaseAssistant(customer, assistant);

            //Step 5: assistant update his location
            await roadsideAssistanceService.UpdateAssistantLocation(assistant, location);

            var statusLog = new List<string>() { $"Customer {customer.Name} Status Logs" };
            statusLog.AddRange(customer.serviceRequest.StatusLog);
            var log = String.Join(Environment.NewLine, statusLog);
            Console.WriteLine(log);
            Assert.True(log.Contains("on the way"));
            Assert.True(log.Contains("released"));

        }
    }
}