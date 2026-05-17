using Avalonia.Controls;
using Avalonia.Interactivity;
using LLMChatbot.ViewModels;
using LLMChatbot.Views;

namespace LLMChatbot.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnSendClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            await vm.SendMessageAsync();

            // Auto-scroll to latest message
            var scrollViewer = this.FindControl<ScrollViewer>("ScrollViewer");
            if (scrollViewer != null)
                scrollViewer.Offset = new Avalonia.Vector(0, double.MaxValue);
        }
    }
}