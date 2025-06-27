using Discord;
using Discord.WebSocket;
using JikanDotNet;

namespace SubaruBOT.Commands
{
    public class RandomTopAnimeCommand
    {
        private readonly IJikan _jikan;
        private readonly Random _random;

        public RandomTopAnimeCommand()
        {
            _jikan = new Jikan();
            _random = new Random();
        }

        public async Task ExecuteAsync(SocketMessage message)
        {
            BaseJikanResponse<ICollection<Anime>> response;

            try
            {
                response = await _jikan.GetTopAnimeAsync();
            }
            catch
            {
                await message.Channel.SendMessageAsync("Could not retrieve the top anime list.");
                return;
            }

            if (response.Data == null || !response.Data.Any())
            {
                await message.Channel.SendMessageAsync("No anime found in the top list.");
                return;
            }

            // Get the top 50 anime and select one randomly
            var topAnime = response.Data.Take(50).ToList();
            var randomAnime = topAnime[_random.Next(topAnime.Count)];

            var embed = new EmbedBuilder()
                .WithTitle($"{randomAnime.Title}")
                .WithImageUrl(randomAnime.Images?.JPG?.ImageUrl ?? "")
                .AddField("Score", $"{randomAnime.Score} ⭐", true)
                .AddField("Type", randomAnime.Type, true)
                .AddField("Episodes", randomAnime.Episodes?.ToString() ?? "Unknown", true)
                .AddField("Status", randomAnime.Status, true)
                .AddField("Year", randomAnime.Year?.ToString() ?? "Unknown", true)
                .AddField("Genres", string.Join(", ", randomAnime.Genres?.Select(g => g.Name) ?? Array.Empty<string>()), true)
                .WithUrl(randomAnime.Url)
                .WithColor(Color.Blue)
                .Build();

            await message.Channel.SendMessageAsync(embed: embed);
        }
    }
}