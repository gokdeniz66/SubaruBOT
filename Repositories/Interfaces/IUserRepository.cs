namespace SubaruBOT.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task EnsureUserExistsAsync(ulong userId);
    }
}