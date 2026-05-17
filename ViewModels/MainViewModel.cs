using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LLMChatbot.Services;

namespace LLMChatbot.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly GeminiService _geminiService;
    private Conversation _conversation;

    private readonly ChatHistoryService _historyService;

    private string _userInput = string.Empty;
    private bool _isBusy;

    // Messages displayed in the UI
    public ObservableCollection<Message> Messages { get; } = new();

    // Bound to the text input box
    public string UserInput
    {
        get => _userInput;
        set { _userInput = value; OnPropertyChanged(); }
    }

    // Disables the send button while waiting for a reply
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    public MainViewModel(GeminiService geminiService)
    {
        _geminiService = geminiService;
        _conversation = new Conversation();
        _historyService = new ChatHistoryService();
    }

    public async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(UserInput) || IsBusy) return;

        var userMessage = UserInput;
        UserInput = string.Empty;

        _conversation.AddMessage("user", userMessage);
        Messages.Add(new Message("user", userMessage));

        IsBusy = true;

        try
        {
            var reply = await _geminiService.SendMessageAsync(_conversation);
            _conversation.AddMessage("model", reply);
            Messages.Add(new Message("model", reply));
        }
        catch (Exception ex)
        {
            var errorMsg = $"❌ Error: {ex.Message}\n\n💡 Tip: Check your API key or try again.";
            Messages.Add(new Message("model", errorMsg));
            _conversation.AddMessage("model", errorMsg);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void ClearChat()
    {
        Messages.Clear();
        _conversation = new Conversation();
    }

    // INotifyPropertyChanged — tells the UI when a property changes
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public async Task SaveChatAsync()
    {
        try
        {
            await _historyService.SaveAsync(_conversation);
            Messages.Add(new Message("model", "💾 Chat saved!"));
        }
        catch (Exception ex)
        {
            Messages.Add(new Message("model", $"❌ Save failed: {ex.Message}"));
        }
    }

    public async Task LoadChatAsync(string fileName)
    {
        try
        {
            _conversation = await _historyService.LoadAsync(fileName);
            Messages.Clear();
            foreach (var msg in _conversation.Messages)
            {
                Messages.Add(msg);
            }
            Messages.Add(new Message("model", "📂 Chat loaded!"));
        }
        catch (Exception ex)
        {
            Messages.Add(new Message("model", $"❌ Load failed: {ex.Message}"));
        }
    }
}