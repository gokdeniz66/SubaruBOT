using Discord;
using Discord.WebSocket;
using SubaruBOT.Services;
using SubaruBOT.Repositories.Interfaces;
using SubaruBOT.Repositories;
using SubaruBOT.Data;

namespace SubaruBOT.Commands
{
    public class GachaCommand
    {
        private readonly IGachaService _gachaService;

        public GachaCommand()
        {
            var config = ConfigService.LoadConfiguration();
            var database = new Database(config);

            IUserRepository userRepository = new UserRepository(database);
            ICharacterRepository characterRepository = new CharacterRepository(database);
            IUserCharacterRepository userCharacterRepository = new UserCharacterRepository(database);

            _gachaService = new GachaService(userRepository, characterRepository, userCharacterRepository);
        }

        public async Task ExecuteAsync(SocketMessage message)
        {
            try
            {
                var userId = message.Author.Id;
                var character = await _gachaService.PullAsync(userId);

                var embed = new EmbedBuilder()
                    .WithTitle($"🎉 You pulled: {character.Name}!")
                    .WithDescription($"Rarity: {GetRaritySymbol(character.Rarity)}")
                    .WithImageUrl(string.IsNullOrEmpty(character.ImageUrl) ? null : character.ImageUrl)
                    .WithColor(GetRarityColor(character.Rarity))
                    .Build();

                await message.Channel.SendMessageAsync(embed: embed);
            }
            catch (Exception ex)
            {
                await message.Channel.SendMessageAsync($"❌ Error: {ex.Message}");
            }
        }

        private string GetRaritySymbol(int rarity) => rarity switch
        {
            0 => "⭐",
            1 => "⭐⭐",
            2 => "🔥🔥",
            3 => "🔥🔥🔥",
            _ => "💥💥💥💥"
        };

        private Color GetRarityColor(int rarity) => rarity switch
        {
            0 => Color.LightGrey,
            1 => Color.Blue,
            2 => Color.Orange,
            3 => Color.Purple,
            _ => Color.Default
        };
    }
}