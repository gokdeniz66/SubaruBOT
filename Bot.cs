using Discord;
using Discord.WebSocket;
using SubaruBOT.Commands;

namespace SubaruBOT
{
    public class Bot
    {
        private DiscordSocketClient _client;

        public async Task StartAsync()
        {

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            });

            _client.Log += OnLogAsync;
            _client.Ready += OnReadyAsync;
            _client.MessageReceived += OnMessageReceivedAsync;

            string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Token empty or not found!");
                return;
            }

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1); // Keep the bot running
        }

        private Task OnLogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task OnReadyAsync()
        {
            Console.WriteLine("Bot is online as" + _client.CurrentUser);
            return Task.CompletedTask;
        }

        private async Task OnMessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.IsBot)
            {
                return;
            }

            if (message.Content.StartsWith("!ping"))
            {
                var command = new PingCommand();
                await command.ExecuteAsync(message);
            }

            if (message.Content.StartsWith("!character"))
            {
                var command = new CharacterCommand();
                await command.ExecuteAsync(message);
            }
        }
    }
}