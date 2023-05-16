using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using RoadsideAssistanceBL.DataStore;
using RoadsideAssistanceBL.Models;
using RoadsideAssistanceBL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideAssistanceBL.IntegrationTests
{
    public class RoadsideAssistanceConcurrentFlowTests
    {
        private IRoadSideDataStore _dataStoreStub;
        private IAssistantService _assistantSevice;
        private ICustomerService _customerSevice;
        private IRoadsideAssistanceService _service; 

        public RoadsideAssistanceConcurrentFlowTests()
        {
            _dataStoreStub = new RoadSideDataStoreStub();
        }

        [SetUp]
        public void Setup()
        {
            _assistantSevice = new AssistantService(_dataStoreStub);
            _customerSevice = new CustomerService(_dataStoreStub);
            _service = new RoadsideAssistanceService(_assistantSevice, _customerSevice);
        }

        [Test]
        public async Task ConcurrentFlowTests()
        {
            /*
                * Concurrent flow
                * Step 1: Create a list of customers with a service request
                * Step 2: Create threads to test each customer flow
                *         a) reserve an assistant
                *         b) release an assitant
                *         c) update assitant location
                * Step 3: Assert with Customer Service log     
   
            */

            //Step 1: Create a list of customers with a service request
            var customers = CreateCustomersWithServiceRequest();

            //Step 2: Create threads to test each customer flow
            var tasks = new List<Task<Customer>>();
            foreach (var customer in customers)
            {
                tasks.Add(TestCustomerFlow(customer));
            }
            await Task.WhenAll();

            //Step 3: Assert with logs
            var json = JsonConvert.SerializeObject(_dataStoreStub.GetAssitants());
            Console.WriteLine($"Final Database Snapshot: {json}");
            foreach (var task in tasks)
            {
               try
                {
                    var customer = task.Result;
                    var log = String.Join(Environment.NewLine, customer.serviceRequest.StatusLog);
                    Assert.True(log.Contains("on the way"), $"Customer {customer.Name} is not assigned with a assitant");
                    Assert.True(log.Contains("released"), $"Customer {customer.Name} is not released the assitant");
                } catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private Task<Customer> TestCustomerFlow(Customer cust)
        {
            return Task<Customer>.Factory.StartNew((obj) =>
            {
                var customer = obj as Customer;

                var statusLog = customer.serviceRequest.StatusLog;
                var random = new Random();
                var location = new Geolocation(random.Next(1, 10), random.Next(1, 10));

                //reserve a assistant
                var assitant =  _service.ReserveAssistant(customer, location).Result;
                if (assitant != null)
                {
                    //after work done release the assistant
                    var doWorkTask = Task.Factory.StartNew(() =>
                    {
                        //Delay 1 to 5 seconds randomly to provide the response
                        var seconds = new Random().Next(3, 5);
                        Thread.Sleep(seconds * 1000);
                    });

                    Task.WaitAll(doWorkTask);
                    _service.ReleaseAssistant(customer, assitant).Wait();
                    location = new Geolocation(random.Next(1, 10), random.Next(1, 10));
                    _service.UpdateAssistantLocation(assitant, location).Wait();
                }
                else
                {
                    statusLog.Add($"{DateTime.Now} No Assitant is allocated");
                    //timeout or error scenario, write retry logic
                }

                ///Step 3: Assert with logs
                var logList = new List<string>() { $"Customer {customer.Name} Status Logs" };
                logList.AddRange(customer.serviceRequest.StatusLog);
                var log = String.Join(Environment.NewLine, logList);
                Console.WriteLine(log);
                var json = JsonConvert.SerializeObject(_dataStoreStub.GetAssitants());
                Console.WriteLine($"{DateTime.Now} Database Snapshot: {json}");
                return customer;
            }, cust, TaskCreationOptions.LongRunning);
        }



        private List<Customer> CreateCustomersWithServiceRequest()
        {
            var customers = new List<Customer>();

            var jack = new Customer()
            {
                Id = 1,
                Name = "Jack",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1001,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "abc"
                    },
                    ProblemDescription = "flat tire"
                }
            };

            var mike = new Customer()
            {
                Id = 2,
                Name = "Mike",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1002,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "xyz"
                    },
                    ProblemDescription = "battery dead"
                }
            };

            var peter = new Customer()
            {
                Id = 3,
                Name = "Peter",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1003,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "pqr"
                    },
                    ProblemDescription = "gas shortage"
                }
            };

            var joseph = new Customer()
            {
                Id = 3,
                Name = "Joseph",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1004,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "mno"
                    },
                    ProblemDescription = "vehicle breakdown"
                }
            };

            var kris = new Customer()
            {
                Id = 3,
                Name = "Kris",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1005,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "efg"
                    },
                    ProblemDescription = "met an accident"
                }
            };

            customers.Add(jack);
            customers.Add(mike);
            customers.Add(peter);
            customers.Add(joseph);
            customers.Add(kris);

            return customers;
        }
    }
}
