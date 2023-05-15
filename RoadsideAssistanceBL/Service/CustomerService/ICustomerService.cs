using RoadsideAssistanceBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideAssistanceBL.Service
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomers();
        Task<bool> IsValidCustomer(int custId);
        Task Save(Customer customer);

    }
}
