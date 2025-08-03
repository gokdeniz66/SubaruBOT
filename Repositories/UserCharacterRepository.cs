using Dapper;
using SubaruBOT.Repositories.Interfaces;
using SubaruBOT.Data;

namespace SubaruBOT.Repositories
{
    public class UserCharacterRepository : IUserCharacterRepository
    {
        private readonly Database _database;

        public UserCharacterRepository(Database database)
        {
            _database = database;
        }

        public async Task InsertUserCharacterAsync(ulong userId, int characterId)
        {
            using var conn = _database.CreateConnection();
            var userIdLong = (long)userId;

            await conn.ExecuteAsync(
                "INSERT INTO UserCharacters (UserId, CharacterId, TimeStamp) VALUES (@UserId, @CharacterId, @TimeStamp)",
                new { UserId = userIdLong, CharacterId = characterId, TimeStamp = DateTime.Now });
        }
    }
}