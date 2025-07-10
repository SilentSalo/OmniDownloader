using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using OmniDownloader.bot.handlers;
using OmniDownloader.bot.config;
using OmniDownloader.bot.tools;

namespace OmniDownloader.bot.commands
{
    public class PrefixCommands : BaseCommandModule
    {
        [Command("download")]
        public async Task DownloadCommand(CommandContext ctx, string message)
        {
            var config = await JsonReader.ReadJsonAsync<JsonStructure>(@"config\config.json");
            var videoUrl = MessageHandler.ExtractUrl(message);

            await Logger.LogAction($"PrefixCommand {config.prefix}download triggered.");
            await CommandHandler.HandlePrefixCommandAsync(ctx, videoUrl);
        }


    }
}
