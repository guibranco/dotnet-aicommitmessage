using AiCommitMessage.Services.AIProviders.Implementation;
using AiCommitMessage.Services.AIProviders.Implementation.LMStudio;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AiCommitMessage.Tests.Units.Services.AIProviders.Implementation;



public class LmStudioAiServiceProviderTests
{
    private const string RealApiEndpoint = "http://localhost:1234/v1";

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _responseContent;

        public MockHttpMessageHandler(HttpStatusCode statusCode, string responseContent)
        {
            _statusCode = statusCode;
            _responseContent = responseContent;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_responseContent, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }

    private bool IsRealApiAvailable()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = httpClient.GetAsync($"{RealApiEndpoint}/models").Result;
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
    
    private LmStudioAiServiceProvider CreateProvider(bool useMock)
    {
        if (useMock)
        {
            // Mock response for models
            var mockModelsResponse = JsonSerializer.Serialize(new
            {
                data = new List<object>
                {
                    new { id = "MockModelA", @object = "model", owned_by = "mock_owner" },
                    new { id = "MockModelB", @object = "model", owned_by = "mock_owner" },
                },
                @object = "list"
            });

            var mockHandler = new MockHttpMessageHandler(HttpStatusCode.OK, mockModelsResponse);
            var mockHttpClient = new HttpClient(mockHandler);

            return new LmStudioAiServiceProvider(mockHttpClient, RealApiEndpoint, "MockModelA");
        }
        else
        {
            var httpClient = new HttpClient();
            return new LmStudioAiServiceProvider(httpClient, RealApiEndpoint, null);
        }
    }

    [Fact]
    public void GetAvailableModels_ShouldUseRealApiIfAvailableOrMockOtherwise()
    {
        // Arrange
        LmStudioAiServiceProvider provider;
        bool useMock = !IsRealApiAvailable();

        if (useMock)
        {
            // Mock setup
            var mockResponseContent = JsonSerializer.Serialize(new
            {
                Models = new List<string> { "MockModelA", "MockModelB", "MockModelC" }
            });
            var mockHandler = new MockHttpMessageHandler(HttpStatusCode.OK, mockResponseContent);
            var mockHttpClient = new HttpClient(mockHandler);

            provider = new LmStudioAiServiceProvider(mockHttpClient, RealApiEndpoint, "MockModelA")
            {
                // This is just an example of how to set up a mock provider
                // You can add more properties to the mock provider if needed
            };
        }
        else
        {
            // Real API setup
            var httpClient = new HttpClient();
            provider = new LmStudioAiServiceProvider(httpClient, RealApiEndpoint, null);
        }

        // Act
        var models = provider.GetAvailableModels();

        // Assert
        Assert.NotNull(models);
        Assert.NotEmpty(models);

        if (useMock)
        {
            // Verify mock models
            Assert.Contains("MockModelA", models);
            Assert.Contains("MockModelB", models);
            Assert.Contains("MockModelC", models);
        }
        else
        {
            // Verify real API models (this is just an example check; adapt to your real API response)
            Assert.DoesNotContain("MockModelA", models);
        }
    }
    
    [Fact]
    public void GenerateAnswer_ShouldUseRealApiIfAvailableOrMockOtherwise()
    {
        // Arrange
        bool useMock = !IsRealApiAvailable();
        var provider = CreateProvider(useMock);

        // Mock response for GenerateAnswer
        if (useMock)
        {
            var mockResponse = JsonSerializer.Serialize(new
            {
                Text = "This is a mocked AI response."
            });

            var mockHandler = new MockHttpMessageHandler(HttpStatusCode.OK, mockResponse);
            var mockHttpClient = new HttpClient(mockHandler);

            provider = new LmStudioAiServiceProvider(mockHttpClient, RealApiEndpoint, "MockModelA");
        }

        // Act
        var response = provider.GenerateAnswer("What is AI?");

        // Assert
        Assert.NotNull(response);

        if (useMock)
        {
            Assert.Equal("This is a mocked AI response.", response);
        }
        else
        {
            // Add assertions for real API response if needed
            Assert.NotEmpty(response);
        }
    }
}
