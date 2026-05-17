namespace LLMChatbot;

public class Message
{
    public string Role { get; set; }    // "user" or "model"
    public string Content { get; set; }

    public Message(string role, string content)
    {
        Role = role;
        Content = content;
    }
}