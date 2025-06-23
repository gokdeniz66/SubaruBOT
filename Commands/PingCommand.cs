using Discord.WebSocket;

namespace SubaruBOT.Commands
{
    public class PingCommand
    {
        public async Task ExecuteAsync(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("Pong!");
        }
    }
}