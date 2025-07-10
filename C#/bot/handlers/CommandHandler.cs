using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using OmniDownloader.bot.tools.context_addon;

namespace OmniDownloader.bot.handlers
{
    public static class CommandHandler
    {
        public static async Task HandlePrefixCommandAsync(CommandContext ctx, string? videoUrl)
        {
            var ctxWrapper = new CommandContextWrapper(ctx);
            await ContextAddon.SendVideoAsync(ctxWrapper, videoUrl);
        }

        public static async Task HandleSlashCommandAsync(InteractionContext ctx, string? videoUrl)
        {
            var ctxWrapper = new InteractionContextWrapper(ctx);
            await ContextAddon.SendVideoAsync(ctxWrapper, videoUrl);
        }

        public static async Task HandleMessageCommandAsync(ContextMenuContext ctx, string? videoUrl)
        {
            var ctxWrapper = new ContextMenuContextWrapper(ctx);
            await ContextAddon.SendVideoAsync(ctxWrapper, videoUrl);
        }


    }
}
