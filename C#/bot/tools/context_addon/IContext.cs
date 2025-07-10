using DSharpPlus.Entities;

namespace OmniDownloader.bot.tools.context_addon
{
    public interface IContext
    {
        Task SendMessageAsync(DiscordMessageBuilder builder);
        ulong MessageId { get; }
        DiscordChannel Channel { get; }
    }

    public interface IInteractionContext : IContext
    {
        Task SendMessageAsync(DiscordWebhookBuilder builder);
    }

}
