using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LLMChatbot.Services;

public class GeminiService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public GeminiService(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
    }

    public async Task<string> SendMessageAsync(Conversation conversation)
    {
        var fullPrompt = string.Join("\n", conversation.Messages
            .Select(m => $"{m.Role}: {m.Content}"));

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_apiKey}";

        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = fullPrompt } } }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API Error - Status: {response.StatusCode}\nResponse: {responseText}");
        }

        try
        {
            var result = JsonDocument.Parse(responseText);
            var text = result.RootElement.GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "No response received.";
        }
        catch (Exception parseEx)
        {
            throw new Exception($"Failed to parse response: {parseEx.Message}\nRaw response: {responseText}");
        }
    }
}