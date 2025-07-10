using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using OmniDownloader.bot.commands;
using OmniDownloader.bot.config;
using OmniDownloader.bot.enums;
using OmniDownloader.bot.handlers;
using OmniDownloader.bot.tools;

namespace OmniDownloader.bot
{
    public static class Bot
    {
        public static DiscordClient? Client { get; private set; }
        public static CommandsNextExtension? PrefixCommands { get; private set; }

        public static async Task RunAsync()
        {
            await CreateLogsAsync();
            await CheckFilesAsync();
            await ConfigAsync();

            if (Client == null) throw new InvalidOperationException("Client is not initialized.");
            Client.Ready += Client_Ready;
            Client.MessageCreated += Client_MessageCreated;
            Client.MessageUpdated += Client_MessageUpdated;
            Client.MessageReactionAdded += Client_MessageReactionAdded;

            await Client.ConnectAsync();
            await Logger.LogAction("Bot connected to the gateway.", ConsoleColor.Green);

            await Task.Delay(-1); // Keeping the bot online
        }

        private static async Task CheckFilesAsync()
        {
            var filePaths = new List<string>
            {
                @"config\config.json",
                @"config\websites.json",
                @"python\download_video.exe"
            };

            foreach (var path in filePaths)
            {
                var directory = Path.GetDirectoryName(path);

                if (!Directory.Exists(directory))
                {
                    throw new FileNotFoundException($"Directory not found: {directory}");
                }

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"File not found: {path}");
                }
            }

            await Logger.LogAction("Bot files validated successfully.", ConsoleColor.Green);
        }

        private static async Task CreateLogsAsync()
        {
            var currentDate = DateTime.Now.ToString("dd_MM_yyyy");
            var logTypes = Enum.GetValues(typeof(LogType));

            foreach (LogType logType in logTypes)
            {
                var filePath = @$"logs\{currentDate}_{logType.ToString().ToLower()}.txt";

                if (!Directory.Exists("logs"))
                {
                    Directory.CreateDirectory("logs");
                }

                if (!File.Exists(filePath))
                {
                    await using var fs = new FileStream(filePath, FileMode.CreateNew);
                }
            }

            await Logger.LogAction("Bot logs created successfully.", ConsoleColor.Green);
        }

        private static async Task ConfigAsync()
        {
            var config = await JsonReader.ReadJsonAsync<JsonStructure>(@"config\config.json");
            var clientConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                TokenType = TokenType.Bot,
                Token = config.token ?? throw new InvalidOperationException("Token is null."),
                AutoReconnect = true,
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.None
            };
            var prefixCommandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { config.prefix ?? throw new InvalidOperationException("Prefix is null.") },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };
            var interactivityConfig = new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            };

            Client = new DiscordClient(clientConfig); // Initialization of discord client
            PrefixCommands = Client.UseCommandsNext(prefixCommandsConfig); // Initialization of prefix commands
            var slashCommandsConfig = Client.UseSlashCommands(); // Initialization of slash commands
            Client.UseInteractivity(interactivityConfig); // Initialization of interactivity

            PrefixCommands.RegisterCommands<PrefixCommands>(); // Registering prefix commands
            slashCommandsConfig.RegisterCommands<SlashCommands>(); // Registering slash commands
            slashCommandsConfig.RegisterCommands<MessageCommands>(); // Registering message commands

            await Logger.LogAction("Bot configured successfully.", ConsoleColor.Green);
        }

        private static async Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            await Logger.LogAction("Bot is online.", ConsoleColor.Green);
        }

        private static async Task Client_MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
        {
            await Logger.LogAction("MessageCreated event triggered.");
            await MessageHandler.HandleMessageCreated(e.Message);
        }

        private static async Task Client_MessageUpdated(DiscordClient sender, MessageUpdateEventArgs e)
        {
            await Logger.LogAction("MessageUpdated event triggered.");
            await MessageHandler.HandleMessageUpdated(e);
        }

        private static async Task Client_MessageReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e)
        {
            await Logger.LogAction("MessageReactionAdded event triggered.");
            await ReactionHandler.HandleReactionAddedAsync(e);
        }


    }
}
