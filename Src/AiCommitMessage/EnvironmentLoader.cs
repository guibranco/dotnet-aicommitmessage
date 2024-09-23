using System;
using System.Security.Cryptography;
using System.Text;

namespace AiCommitMessage
{
    public static class EnvironmentLoader
    {
        public static string LoadOpenAIApiUrl()
        {
            return Environment.GetEnvironmentVariable("OPENAI_API_URL") ?? "https://api.openai.com";
        }

        public static string LoadAndDecryptOpenAIApiKey()
        {
            var encryptedKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(encryptedKey))
            {
                throw new InvalidOperationException("The OPENAI_API_KEY environment variable is not set.");
            }
            return Decrypt(encryptedKey);
        }

        public static string LoadOptionalEmoji()
        {
            return Environment.GetEnvironmentVariable("MESSAGE_EMOJI") ?? string.Empty;
        }

        private static string Decrypt(string encryptedText)
        {
            // Placeholder for decryption logic
            // Implement your decryption logic here
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
        }
    }
}
