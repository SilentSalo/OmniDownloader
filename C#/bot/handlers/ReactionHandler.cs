using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using OmniDownloader.bot.tools.context_addon;
using System.Collections.Concurrent;

namespace OmniDownloader.bot.handlers
{
    public static class ReactionHandler
    {
        private static readonly ConcurrentDictionary<ulong, HashSet<ulong>> ProcessedReactions = new();

        public static int CountReactions(DiscordMessage message, DiscordEmoji emoji)
        {
            var reaction = message.Reactions.FirstOrDefault(r => r.Emoji == emoji);
            return reaction?.Count ?? 0;
        }

        public static async Task HandleReactionAddedAsync(MessageReactionAddEventArgs e)
        {
            var emoji = DiscordEmoji.FromName(Bot.Client, ":arrow_down:");

            if (e.Emoji.Name == emoji && !e.User.IsBot && CountReactions(e.Message, emoji) == 2)
            {
                if (ProcessedReactions.TryGetValue(e.Message.Id, out var users) && users.Contains(e.User.Id))
                {
                    return;
                }

                ProcessedReactions.AddOrUpdate(e.Message.Id, [e.User.Id], (key, oldValue) =>
                {
                    oldValue.Add(e.User.Id);
                    return oldValue;
                });

                var ctxWrapper = new ReactionContextWrapper(e);
                var videoUrl = MessageHandler.ExtractUrl(e.Message.Content);
                
                await ContextAddon.SendVideoAsync(ctxWrapper, videoUrl);
            }
        }


    }
}
