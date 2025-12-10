using System;
using System.Threading.Tasks;
using Spectre.Console;

namespace AiCommitMessage.Services.Cache
{
    public class CommitMessageCacheService
    {
        private readonly ICacheProvider _cacheProvider;

        public CommitMessageCacheService(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public async Task<string> GetOrGenerateAsync(
            string model,
            string branch,
            string authorMessage,
            string diff,
            Func<Task<string>> generateFunc
        )
        {
            string hash = _cacheProvider.GenerateHash(model, branch, authorMessage, diff);
            string? cached = await _cacheProvider.LoadAsync(model, hash);
            if (cached != null)
            {
                AnsiConsole.WriteLine("\u2705 Loaded from cache.");
                return cached;
            }

            string result = await generateFunc();
            await _cacheProvider.SaveAsync(model, hash, result);
            AnsiConsole.WriteLine("\ud83d\udcac Cached new result.");
            return result;
        }
    }
}