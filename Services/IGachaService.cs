using SubaruBOT.Models;

namespace SubaruBOT.Services
{
    public interface IGachaService
    {
        Task<CharacterDb> PullAsync(ulong userId);
    }
}