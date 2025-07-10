using DSharpPlus;
using DSharpPlus.SlashCommands;
using OmniDownloader.bot.handlers;
using OmniDownloader.bot.tools;

namespace OmniDownloader.bot.commands
{
    public class MessageCommands : ApplicationCommandModule
    {
        [ContextMenu(ApplicationCommandType.MessageContextMenu, "Download")]
        public async Task DownloadCommand(ContextMenuContext ctx)
        {
            var videoUrl = MessageHandler.ExtractUrl(ctx.TargetMessage.Content);

            await Logger.LogAction("MessageCommand Download triggered.");
            await ctx.DeferAsync();
            await CommandHandler.HandleMessageCommandAsync(ctx, videoUrl);
        }


    }
}
