using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using LLMChatbot.ViewModels;

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

            var scrollViewer = this.FindControl<ScrollViewer>("ScrollViewer");
            if (scrollViewer != null)
                scrollViewer.Offset = new Avalonia.Vector(0, double.MaxValue);
        }
    }

    private void OnClearClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.ClearChat();
        }
    }
    private async void OnSaveClicked(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            await vm.SaveChatAsync();
        }
    }
}