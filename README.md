# LLM Chatbot

A desktop chatbot that integrates with Google's Gemini AI. Built with C# and Avalonia.

## Features

- Real-time chat with Gemini AI
- Message timestamps
- Save conversations as JSON
- Clear chat history
- Error recovery

## Tech Stack

- C# (.NET 10)
- Avalonia UI Framework
- Google Gemini 2.5 Flash API
- MVVM Architecture Pattern

## Setup

1. Clone the repository
2. Install .NET 10 SDK
3. Get your own API key from [Google AI Studio](https://aistudio.google.com/apikey)
4. Create a `.env` file in the project root with your API key (kept private, never shared)
5. Run: `dotnet run`

## How It Works

- Type a message and click **Send** to chat with AI
- Click **Save** to export conversation to JSON
- Click **Clear** to start a new chat
- Messages include timestamps
- Conversations stored in: `~/Documents/LLMChatbot/History/`

## Project Structure

- `Models/` - Message and Conversation classes
- `Services/` - API integration and file saving
- `ViewModels/` - Application logic (MVVM)
- `Views/` - User interface (Avalonia XAML)
- `Converters/` - Custom UI value converters

## OOP Principles Demonstrated

✅ Encapsulation - Private fields, public properties
✅ MVVM Pattern - Separation of UI and logic
✅ Async Programming - Non-blocking API calls
✅ Error Handling - User-friendly error messages
✅ Data Persistence - Save/load conversations

## Security Note

API keys are stored in `.env` files and never committed to Git. Always keep credentials private.
