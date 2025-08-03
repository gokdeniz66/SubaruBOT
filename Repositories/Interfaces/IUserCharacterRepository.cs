namespace SubaruBOT.Repositories.Interfaces
{
    public interface IUserCharacterRepository
    {
        Task InsertUserCharacterAsync(ulong userId, int characterId);
    }
}