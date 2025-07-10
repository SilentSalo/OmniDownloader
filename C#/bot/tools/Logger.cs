using DSharpPlus.Entities;
using OmniDownloader.bot.enums;
using System.Runtime.CompilerServices;

namespace OmniDownloader.bot.tools
{
    public static class Logger
    {
        private static void Log(string message, ConsoleColor color = ConsoleColor.Magenta)
        {
            var currentTime = DateTime.Now.ToString("HH:mm:ss");

            Console.ForegroundColor = color;
            Console.WriteLine($"{currentTime}".PadRight(13) + message);
            Console.ResetColor();
        }

        private static async Task LogToFile(string logMessage, LogType logType)
        {
            var currentDate = DateTime.Now.ToString("dd_MM_yyyy");
            var currentTime = DateTime.Now.ToString("HH:mm:ss");
            string filePath = @$"logs\{currentDate}_{logType.ToString().ToLower()}.txt";

            using var sw = new StreamWriter(filePath, true);

            await sw.WriteLineAsync($"{currentTime}".PadRight(13) + $"{logMessage}");
        }

        public static async Task LogMessage(DiscordMessage message, ConsoleColor color = ConsoleColor.Yellow)
        {
            string serverName = message.Channel.Guild?.Name ?? "DM";
            string channelName = message.Channel.Name ?? "DM";
            
            if (message.Content.Length > 0)
            {
                string logMessage = $"{message.Author.Username} ({serverName}, #{channelName}): {message.Content}";

                Log(logMessage, color);
                await LogToFile(logMessage, LogType.Messages);
                await LogToFile(logMessage, LogType.All);
            }
        }

        public static async Task LogAction(string message, ConsoleColor color = ConsoleColor.Cyan, [CallerMemberName] string? callerName = null, [CallerFilePath] string? filePath = null)
        {
            string? className = Path.GetFileNameWithoutExtension(filePath);
            string logMessage = $"{className}.{callerName}()".PadRight(42) + $"{message}";

            Log(logMessage, color);
            await LogToFile(logMessage, LogType.Actions);
            await LogToFile(logMessage, LogType.All);
        }


    }
}
