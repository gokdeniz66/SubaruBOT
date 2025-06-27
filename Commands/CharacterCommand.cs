using Discord;
using Discord.WebSocket;
using JikanDotNet;

namespace SubaruBOT.Commands
{
    public class CharacterCommand
    {
        private readonly IJikan _jikan;
        private readonly Random _random;
        private readonly int[] ReZeroIds = { 31240, 38414, 36286, 39587, 42203, 54857 };

        public CharacterCommand()
        {
            _jikan = new Jikan();
            _random = new Random();
        }

        public async Task ExecuteAsync(SocketMessage message)
        {
            int selectedId = ReZeroIds[_random.Next(ReZeroIds.Length)];

            var result = await _jikan.GetAnimeCharactersAsync(selectedId);
            var characters = result.Data;

            if (characters == null || !characters.Any())
            {
                await message.Channel.SendMessageAsync("No characters found!");
                return;
            }

            var character = characters.ElementAt(_random.Next(characters.Count));

            var embed = new EmbedBuilder()
                .WithTitle(character.Character.Name)
                .WithUrl($"https://myanimelist.net/character/{character.Character.MalId}")
                .WithDescription($"**Role:** {character.Role}")
                .WithImageUrl(character.Character.Images?.JPG?.ImageUrl ?? "")
                .WithColor(Color.Blue)
                .Build();

            await message.Channel.SendMessageAsync(embed: embed);
        }
    }
}