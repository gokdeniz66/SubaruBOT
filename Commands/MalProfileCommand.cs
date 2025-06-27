using Discord;
using Discord.WebSocket;
using JikanDotNet;

namespace SubaruBOT.Commands
{
    public class MalProfileCommand
    {
        private readonly IJikan _jikan;

        public MalProfileCommand()
        {
            _jikan = new Jikan();
        }

        public async Task ExecuteAsync(SocketMessage message, string username)
        {
            BaseJikanResponse<UserProfile> response;
            try
            {
                response = await _jikan.GetUserProfileAsync(username);
            }
            catch
            {
                await message.Channel.SendMessageAsync($"Could not found the following user: {username}");
                return;
            }

            if (response.Data == null)
            {
                await message.Channel.SendMessageAsync($"Could not found the following user: {username}");
                return;
            }

            var user = response.Data;

            BaseJikanResponse<UserStatistics> statsResponse;
            try
            {
                statsResponse = await _jikan.GetUserStatisticsAsync(username);
            }
            catch
            {
                await message.Channel.SendMessageAsync($"Could not found the statics for the following user: {username}");
                return;
            }

            int watching = statsResponse.Data.AnimeStatistics.Watching ?? 0;
            int amountEpisodesCompleted = statsResponse.Data.AnimeStatistics.EpisodesWatched ?? 0;

            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username}'s Profile")
                .AddField("Watching", $"{watching} anime(s)", inline: true)
                .AddField("Completed", $"{amountEpisodesCompleted} episodes", inline: true)
                .WithUrl($"https://myanimelist.net/profile/{user.Username}")
                .WithImageUrl(user.Images.JPG.ImageUrl ?? "https://cdn.myanimelist.net/images/kaomoji_mal_white.png")
                .WithFooter($"Location: {user.Location ?? "Kingdom of Lugunica"}\nLast Online: {user.LastOnline:MMMM dd, yyyy}")
                .WithColor(Color.Purple)
                .Build();

            await message.Channel.SendMessageAsync(embed: embed);
        }
    }
}