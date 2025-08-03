using Dapper;
using SubaruBOT.Repositories.Interfaces;
using SubaruBOT.Data;

namespace SubaruBOT.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Database _database;

        public UserRepository(Database database)
        {
            _database = database;
        }

        public async Task EnsureUserExistsAsync(ulong userId)
        {
            using var conn = _database.CreateConnection();
            var userIdLong = (long)userId;

            var exists = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Users WHERE UserId = @UserId",
                new { UserId = userIdLong });

            if (exists == 0)
            {
                await conn.ExecuteAsync(
                    "INSERT INTO Users (UserId) VALUES (@UserId)",
                    new { UserId = userIdLong });
            }
        }
    }
}