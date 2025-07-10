using DSharpPlus.Entities;
using OmniDownloader.bot.config;

namespace OmniDownloader.bot.tools.context_addon
{
    public static class ContextAddon
    {
        public static async Task SendVideoAsync(IContext ctx, string? videoUrl)
        {
            bool videoUrlContainsWebsite = false;

            if (videoUrl == null)
            {
                if (ctx is IInteractionContext interactionCtx)
                {
                    var builder = new DiscordWebhookBuilder().WithContent("Video URL not found.");
                    await interactionCtx.SendMessageAsync(builder);
                }
                else
                {
                    var builder = new DiscordMessageBuilder().WithReply(ctx.MessageId).WithContent("Video URL not found.");
                    await ctx.SendMessageAsync(builder);
                }

                return;
            }
            else
            {
                var websites = await JsonReader.ReadJsonAsync<List<string>>(@"config\websites.json");

                foreach (var website in websites) 
                { 
                    if (videoUrl.Contains(website))
                    {
                        videoUrlContainsWebsite = true;
                    }
                }
            }

            if (videoUrlContainsWebsite)
            {
                var exePath = @"python\download_video.exe";
                var videoBytes = await Python.GetVideoBytesAsync(exePath, videoUrl);

                if (videoBytes != null && videoBytes.Length > 0)
                {
                    using var ms = new MemoryStream(videoBytes);

                    long fileSizeInBytes = ms.Length;
                    double fileSizeInMB = fileSizeInBytes / 1048576.0;

                    if (fileSizeInMB <= 25.0)
                    {
                        if (ctx is IInteractionContext interactionCtx)
                        {
                            var builder = new DiscordWebhookBuilder().AddFile("OmniVideo.mp4", ms);
                            await interactionCtx.SendMessageAsync(builder);
                        }
                        else
                        {
                            var builder = new DiscordMessageBuilder().WithReply(ctx.MessageId).AddFile("OmniVideo.mp4", ms);
                            await ctx.SendMessageAsync(builder);
                        }

                        await Logger.LogAction($"Can send video: size is {fileSizeInMB.ToString("F2")} MB.", ConsoleColor.Magenta);
                    }
                    else
                    {
                        if (ctx is IInteractionContext interactionCtx)
                        {
                            var builder = new DiscordWebhookBuilder().WithContent("Can't send video: size is more than 25 MB.");
                            await interactionCtx.SendMessageAsync(builder);
                        }
                        else
                        {
                            var builder = new DiscordMessageBuilder().WithReply(ctx.MessageId).WithContent("Can't send video: size is more than 25 MB.");
                            await ctx.SendMessageAsync(builder);
                        }

                        await Logger.LogAction("Can't send video: size is more than 25 MB.", ConsoleColor.Magenta);
                    }

                }
                else
                {
                    if (ctx is IInteractionContext interactionCtx)
                    {
                        var builder = new DiscordWebhookBuilder().WithContent("Can't send the video: size is 0 MB.");
                        await interactionCtx.SendMessageAsync(builder);
                    }
                    else
                    {
                        var builder = new DiscordMessageBuilder().WithReply(ctx.MessageId).WithContent("Can't send the video: size is 0 MB.");
                        await ctx.SendMessageAsync(builder);
                    }

                    await Logger.LogAction("Can't send the video: size is 0 MB.", ConsoleColor.Magenta);
                }
            }
            else
            {
                if (ctx is IInteractionContext interactionCtx)
                {
                    var builder = new DiscordWebhookBuilder().WithContent("Video URL found, but website isn't reqistered.");
                    await interactionCtx.SendMessageAsync(builder);
                }
                else
                {
                    var builder = new DiscordMessageBuilder().WithReply(ctx.MessageId).WithContent("Video URL found, but website isn't reqistered.");
                    await ctx.SendMessageAsync(builder);
                }

                await Logger.LogAction("Video URL found, but website isn't reqistered.", ConsoleColor.Magenta);
            }
        }


    }
}
