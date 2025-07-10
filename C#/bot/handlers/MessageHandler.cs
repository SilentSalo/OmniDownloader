using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using OmniDownloader.bot.config;
using OmniDownloader.bot.tools;
using System.Text.RegularExpressions;

namespace OmniDownloader.bot.handlers
{
    public static class MessageHandler
    {
        public static string? ExtractUrl(string message)
        {
            var regex = new Regex(@"https?://[^\s]+");
            var match = regex.Match(message);

            return match.Success ? match.Value : null;
        }

        public static async Task HandleMessageCreated(DiscordMessage message)
        {
            await Logger.LogMessage(message);

            var config = await JsonReader.ReadJsonAsync<JsonStructure>(@"config\config.json");
            var websites = await JsonReader.ReadJsonAsync<List<string>>(@"config\websites.json");

            foreach (var website in websites)
            {
                if (!message.Content.Contains($"{config.prefix}download") && message.Content.Contains(website))
                {
                    var emoji = DiscordEmoji.FromName(Bot.Client, ":arrow_down:");

                    await message.CreateReactionAsync(emoji);
                    await Logger.LogAction("MessageHandler reaction added.");
                    break;
                }
            }
        }

        public static async Task HandleMessageUpdated(MessageUpdateEventArgs e)
        {
            await HandleMessageCreated(e.Message);
        }


    }
}
