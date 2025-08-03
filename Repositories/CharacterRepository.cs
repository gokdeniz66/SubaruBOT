using Dapper;
using SubaruBOT.Repositories.Interfaces;
using SubaruBOT.Models;
using SubaruBOT.Data;

namespace SubaruBOT.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly Database _database;

        public CharacterRepository(Database database)
        {
            _database = database;
        }

        public async Task<IEnumerable<CharacterDb>> GetCharactersByRarityAsync(int rarity)
        {
            using var conn = _database.CreateConnection();
            return await conn.QueryAsync<CharacterDb>(
                "SELECT * FROM Characters WHERE Rarity = @Rarity",
                new { Rarity = rarity });
        }
    }
}