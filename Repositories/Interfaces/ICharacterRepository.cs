using SubaruBOT.Models;

namespace SubaruBOT.Repositories.Interfaces
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<CharacterDb>> GetCharactersByRarityAsync(int rarity);
    }
}