using SubaruBOT.Models;
using SubaruBOT.Repositories.Interfaces;

namespace SubaruBOT.Services
{
    public class GachaService : IGachaService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IUserCharacterRepository _userCharacterRepository;
        private readonly Random _random;

        public GachaService(
            IUserRepository userRepository,
            ICharacterRepository characterRepository,
            IUserCharacterRepository userCharacterRepository)
        {
            _userRepository = userRepository;
            _characterRepository = characterRepository;
            _userCharacterRepository = userCharacterRepository;
            _random = new Random();
        }

        public async Task<CharacterDb> PullAsync(ulong userId)
        {
            await _userRepository.EnsureUserExistsAsync(userId);

            int rarity = GetRandomRarity();
            var charactersDb = (await _characterRepository.GetCharactersByRarityAsync(rarity)).ToList();

            if (!charactersDb.Any())
            {
                throw new InvalidOperationException("No characters found for the selected rarity.");
            }

            var characterDb = charactersDb[_random.Next(charactersDb.Count)];

            await _userCharacterRepository.InsertUserCharacterAsync(userId, characterDb.Id);
            return characterDb;
        }

        private int GetRandomRarity()
        {
            var roll = _random.Next(1, 5);

            return roll switch
            {
                1 => 1, // 25% common
                2 => 2, // 25% rare
                3 => 3, // 25% epic
                4 => 4  // 25% legendary
            };
        }
    }
}