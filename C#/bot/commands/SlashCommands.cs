using DSharpPlus.SlashCommands;
using OmniDownloader.bot.handlers;
using OmniDownloader.bot.tools;

namespace OmniDownloader.bot.commands
{
    public class SlashCommands : ApplicationCommandModule
    {
        [SlashCommand("download", "Download almost any video without watermark.")]
        public async Task DownloadCommand(InteractionContext ctx, [Option("link", "Link to the video.")] string message)
        {
            var videoUrl = MessageHandler.ExtractUrl(message);

            await Logger.LogAction($"SlashCommand /download triggered.");
            await ctx.DeferAsync();
            await CommandHandler.HandleSlashCommandAsync(ctx, videoUrl);
        }


    }
}
