namespace RoadsideAssistanceApi.Repository
{
    public interface ICustomerRepository
    {
        Task<bool> IsValidCustomer(int custId);
    }
}
