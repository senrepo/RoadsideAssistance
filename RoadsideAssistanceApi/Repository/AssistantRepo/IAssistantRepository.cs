namespace RoadsideAssistanceApi.Repository
{
    public interface IAssistantRepository
    {
        Task<bool> IsValidAssistant(int assistantId);
    }
}
