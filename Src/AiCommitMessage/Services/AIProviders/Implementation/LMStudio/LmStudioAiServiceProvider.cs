using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AiCommitMessage.Services.AIProviders.Core;

namespace AiCommitMessage.Services.AIProviders.Implementation;

public class LmStudioAiServiceProvider : IAiServiceProvider
{
    private readonly string _apiEndpoint;
    private readonly string _modelName;
    private readonly HttpClient _httpClient;

    public LmStudioAiServiceProvider(HttpClient httpClient,string apiEndpoint, string modelName)
    {
        _httpClient = httpClient;
        
        if (string.IsNullOrEmpty(apiEndpoint))
        {
            apiEndpoint = "http://localhost:1234/v1";
        }
        _apiEndpoint = apiEndpoint;
        if (string.IsNullOrEmpty(modelName))
        {
            
            var model = GetAvailableModels().FirstOrDefault();
            if (model == null)
            {
                throw new ArgumentException("We could not find and model loaded on the LM Studio Api", nameof(modelName));
            }
            modelName = model;
        }

       
        _modelName = modelName;
        
    }
    
    public List<string> GetAvailableModels()
    {
        try
        {
            // Define the endpoint for querying models
            var modelsEndpoint = $"{_apiEndpoint}/models";

            // Send GET request to the endpoint
            var response = _httpClient.GetAsync(modelsEndpoint).Result;
            response.EnsureSuccessStatusCode();

            // Parse the response
            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var modelsResponse = JsonSerializer.Deserialize<LmStudioModelsResponse>(jsonResponse);
            
            var resultModel = modelsResponse?.Data.Select(x => x.Id).ToList();

            // Return the list of models
            return resultModel ?? new List<string>();
        }
        catch (Exception ex)
        {
            // Handle errors during API call
            throw new InvalidOperationException("Error retrieving available models.", ex);
        }
    }

// Helper class to deserialize response
    public class LmStudioModelsResponse
    {
        [JsonPropertyName("data")]
        public Datum[] Data { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }
    }

    public class Datum
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("owned_by")]
        public string OwnedBy { get; set; }
    }

    public string GenerateAnswer(string prompt)
    {
        if (string.IsNullOrEmpty(prompt))
        {
            throw new ArgumentException("Prompt cannot be null or empty.", nameof(prompt));
        }

        // Prepare request payload
        var payload = new
        {
            model = _modelName,
            prompt = prompt,
            temperature = 0.7,
            maxTokens = 512
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        var endpoint = $"{_apiEndpoint}/completions";

        try
        {
            // Send request to LM Studio API
            var response = _httpClient.PostAsync(endpoint, content).Result;
            response.EnsureSuccessStatusCode();

            // Parse response
            var jsonResponse = response.Content.ReadAsStringAsync().Result;
            var responseObject = JsonSerializer.Deserialize<LmCompletationResponseModel>(jsonResponse);
            
            var testResponse = string.Join("\n", responseObject?.Choices.Select(x => x.Text) ?? Enumerable.Empty<string>());


            return testResponse ?? "No response from AI.";
        }
        catch (Exception ex)
        {
            // Handle API call errors
            throw new InvalidOperationException("Error during AI response generation.", ex);
        }
    }
}

public class LmCompletationResponseModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; }

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("choices")]
    public Choice[] Choices { get; set; }

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; }
}

public class Choice
{
    [JsonPropertyName("index")]
    public long Index { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("logprobs")]
    public object Logprobs { get; set; }

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; }
}

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public long PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public long CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public long TotalTokens { get; set; }
}

