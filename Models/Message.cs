using System;

namespace LLMChatbot;

public class Message
{
    public string Role { get; set; }    // "user" or "model"
    public string Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public Message(string role, string content)
    {
        Role = role;
        Content = content;
        Timestamp = DateTime.Now;
    }
}