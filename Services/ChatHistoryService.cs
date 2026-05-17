using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LLMChatbot.Services;

public class ChatHistoryService
{
    private readonly string _historyFolder;

    public ChatHistoryService()
    {
        _historyFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "LLMChatbot",
            "History"
        );
        Directory.CreateDirectory(_historyFolder);
    }

    public async Task SaveAsync(Conversation conversation, string fileName = null)
    {
        fileName ??= $"chat_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
        var filePath = Path.Combine(_historyFolder, fileName);

        var messages = new List<object>();
        foreach (var msg in conversation.Messages)
        {
            messages.Add(new
            {
                msg.Role,
                msg.Content,
                msg.Timestamp
            });
        }

        var json = JsonSerializer.Serialize(messages, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<Conversation> LoadAsync(string fileName)
    {
        var filePath = Path.Combine(_historyFolder, fileName);
        
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Chat history not found: {fileName}");

        var json = await File.ReadAllTextAsync(filePath);
        var messages = JsonSerializer.Deserialize<List<JsonElement>>(json);

        var conversation = new Conversation();
        foreach (var msg in messages)
        {
            var role = msg.GetProperty("role").GetString();
            var content = msg.GetProperty("content").GetString();
            conversation.AddMessage(role, content);
        }

        return conversation;
    }

    public List<string> GetSavedChats()
    {
        var files = Directory.GetFiles(_historyFolder, "*.json");
        return new List<string>(files.Select(Path.GetFileName));
    }
}