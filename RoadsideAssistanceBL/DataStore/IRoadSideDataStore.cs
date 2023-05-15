using RoadsideAssistanceBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideAssistanceBL.DataStore
{
    public interface IRoadSideDataStore
    {
        List<Assistant> GetAssitants();
        List<Customer> GetCustomers();
    }
}
