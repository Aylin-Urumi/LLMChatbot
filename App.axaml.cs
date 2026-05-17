using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DotNetEnv;
using LLMChatbot.Services;
using LLMChatbot.ViewModels;
using LLMChatbot.Views;

namespace LLMChatbot;

public partial class App : Application
{
    private static string LogFile => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "chatbot_debug.txt");

    private void Log(string message)
    {
        try
        {
            File.AppendAllText(LogFile, $"{DateTime.Now:HH:mm:ss} - {message}\n");
        }
        catch { }
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        try
        {
            Log("=== App Starting ===");
            Env.Load();
            Log(".env loaded");

            var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("API key not found!");

            Log("Creating GeminiService...");
            var geminiService = new GeminiService(apiKey);
            Log("GeminiService created!");

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel(geminiService)
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        catch (Exception ex)
        {
            Log($"ERROR: {ex.Message}");
            throw;
        }
    }
}