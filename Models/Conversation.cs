using System.Collections.Generic;

namespace LLMChatbot;

public class Conversation
{
    public List<Message> Messages { get; set; } = new();

    public void AddMessage(string role, string content)
    {
        Messages.Add(new Message(role, content));
    }
}