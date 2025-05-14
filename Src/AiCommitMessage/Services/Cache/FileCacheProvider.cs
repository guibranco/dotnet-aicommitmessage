using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace AiCommitMessage.Services.Cache
{
    public class FileCacheProvider : ICacheProvider
    {
        private record CachedResponse(
            string Model,
            string Response,
            string Checksum,
            DateTime Timestamp
        );

        private readonly string _cacheDir;

        public FileCacheProvider(string appName = "CommitMessageTool")
        {
            string? envPath = Environment.GetEnvironmentVariable("COMMIT_CACHE_PATH");
            if (!string.IsNullOrWhiteSpace(envPath))
            {
                _cacheDir = Path.Combine(envPath, appName, "commit-cache");
            }
            else
            {
                string baseDir = Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData
                );
                if (string.IsNullOrWhiteSpace(baseDir))
                {
                    string? home =
                        Environment.GetEnvironmentVariable("HOME")
                        ?? Environment.GetEnvironmentVariable("USERPROFILE");
                    if (string.IsNullOrEmpty(home))
                        throw new InvalidOperationException(
                            "Unable to determine a valid user data directory"
                        );
                    baseDir = Path.Combine(home, ".config");
                }
                _cacheDir = Path.Combine(baseDir, appName, "commit-cache");
            }
            Directory.CreateDirectory(_cacheDir);
        }

        public string GenerateHash(string model, string branch, string authorMessage, string diff)
        {
            string combined = $"{model}|{branch}|{authorMessage}|{diff}";
            using var sha = SHA256.Create();
            byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        public async Task<string?> LoadAsync(string model, string hash, int maxAgeDays = 30)
        {
            string path = Path.Combine(_cacheDir, $"{hash}.json");
            if (!File.Exists(path))
                return null;

            var json = await File.ReadAllTextAsync(path);
            var cached = JsonSerializer.Deserialize<CachedResponse>(json);
            if (cached is null || cached.Model != model)
                return null;

            bool expired = DateTime.UtcNow - cached.Timestamp > TimeSpan.FromDays(maxAgeDays);
            bool validChecksum = cached.Checksum == ComputeChecksum(model, cached.Response);

            if (expired || !validChecksum)
            {
                File.Delete(path);
                return null;
            }
            return cached.Response;
        }

        public async Task SaveAsync(string model, string hash, string response)
        {
            string path = Path.Combine(_cacheDir, $"{hash}.json");
            var checksum = ComputeChecksum(model, response);
            var cached = new CachedResponse(model, response, checksum, DateTime.UtcNow);
            var json = JsonSerializer.Serialize(cached);
            await File.WriteAllTextAsync(path, json);
        }

        private string ComputeChecksum(string model, string content)
        {
            using var sha = SHA256.Create();
            var combined = $"{model}|{content}";
            byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
