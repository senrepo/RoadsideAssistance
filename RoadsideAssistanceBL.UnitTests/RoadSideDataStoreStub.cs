using RoadsideAssistanceBL.DataStore;
using RoadsideAssistanceBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideAssistanceBL.UnitTests
{
    public class RoadSideDataStoreStub : RoadSideDataStore
    {
        public override List<Assistant> GetAssitants()
        {
            if (_assistants.Count == 0) _assistants = LoadAssitants();
            return _assistants;
        }

        public override List<Customer> GetCustomers()
        {
            if (_customers.Count == 0) _customers = LoadCustomers();
            return _customers;
        }

        private List<Assistant> LoadAssitants()
        {
            var list = new List<Assistant>()
                {
                    new Assistant()
                    {
                        Id = 1,
                        Name = "Rajesh",
                        IsOccupied = false
                    },
                    new Assistant()
                    {
                        Id = 2,
                        Name = "Muthu",
                        IsOccupied = false
                    },
                    new Assistant()
                    {
                        Id = 3,
                        Name = "Pawn",
                        IsOccupied = false
                    }
            };

            foreach (var assistant in list)
            {
                assistant.MakeAvailable();
            }

            return list;
        }

        private List<Customer> LoadCustomers()
        {
            return new List<Customer>()
                {
                    new Customer()
                    {
                        Id = 1,
                        Name = "Jack",
                        PhoneNumber = "1234567891"

                    },
                    new Customer()
                    {
                        Id = 2,
                        Name = "Mike",
                        PhoneNumber = "1234567892"
                    },
                    new Customer()
                    {
                        Id = 3,
                        Name = "Peter",
                        PhoneNumber = "1234567893"
                    },
                    new Customer()
                    {
                        Id = 4,
                        Name = "Joseph",
                        PhoneNumber = "1234567894"
                    },
                    new Customer()
                    {
                        Id = 5,
                        Name = "Kris",
                        PhoneNumber = "1234567895"
                    }
            };
        }
    }
}
