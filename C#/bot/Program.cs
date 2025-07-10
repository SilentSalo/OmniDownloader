using OmniDownloader.bot;
using OmniDownloader.bot.tools;

namespace OmniDownloader
{
    static class Program
    {
        static async Task Main()
        {
            try
            {
                await Bot.RunAsync();
            }
            catch (Exception ex)
            {
                await Logger.LogAction(ex.Message, ConsoleColor.Red);
            }
        }
    }
}