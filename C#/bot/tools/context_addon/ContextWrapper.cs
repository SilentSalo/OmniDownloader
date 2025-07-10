using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;

namespace OmniDownloader.bot.tools.context_addon
{
    public class CommandContextWrapper(CommandContext ctx) : IContext
    {
        public Task SendMessageAsync(DiscordMessageBuilder builder) => ctx.Channel.SendMessageAsync(builder);
        public ulong MessageId => ctx.Message.Id;
        public DiscordChannel Channel => ctx.Channel;
    }

    public class InteractionContextWrapper(InteractionContext ctx) : IInteractionContext
    {
        public Task SendMessageAsync(DiscordMessageBuilder builder) => ctx.Channel.SendMessageAsync(builder);
        public Task SendMessageAsync(DiscordWebhookBuilder builder) => ctx.EditResponseAsync(builder);
        public ulong MessageId => ctx.Interaction.Id;
        public DiscordChannel Channel => ctx.Channel;
    }

    public class ContextMenuContextWrapper(ContextMenuContext ctx) : IInteractionContext
    {
        public Task SendMessageAsync(DiscordMessageBuilder builder) => ctx.Channel.SendMessageAsync(builder);
        public Task SendMessageAsync(DiscordWebhookBuilder builder) => ctx.EditResponseAsync(builder);
        public ulong MessageId => ctx.Interaction.Id;
        public DiscordChannel Channel => ctx.Channel;
    }

    public class ReactionContextWrapper(MessageReactionAddEventArgs e) : IContext
    {
        public Task SendMessageAsync(DiscordMessageBuilder builder) => e.Channel.SendMessageAsync(builder);
        public ulong MessageId => e.Message.Id;
        public DiscordChannel Channel => e.Channel;
    }


}
