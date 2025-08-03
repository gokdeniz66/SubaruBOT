using SubaruBOT.Models;

namespace SubaruBOT.Services.Interfaces
{
    public interface IGachaService
    {
        Task<CharacterDb> PullAsync(ulong userId);
    }
}