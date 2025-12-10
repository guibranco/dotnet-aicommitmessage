using System.Threading.Tasks;

namespace AiCommitMessage.Services.Cache
{
    public interface ICacheProvider
    {
        string GenerateHash(string model, string branch, string authorMessage, string diff);
        Task<string?> LoadAsync(string model, string hash, int maxAgeDays = 30);
        Task SaveAsync(string model, string hash, string response);
    }
}