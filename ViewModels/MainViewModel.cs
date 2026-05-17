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
    private readonly Conversation _conversation;

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
    }

    public async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(UserInput) || IsBusy) return;

        var userMessage = UserInput;
        UserInput = string.Empty; // Clear input box immediately

        // Add to history and UI
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
            Messages.Add(new Message("model", $"Error: {ex.Message}"));
        }
        finally
        {
            IsBusy = false; // Always re-enable the button
        }
    }

    // INotifyPropertyChanged — tells the UI when a property changes
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}