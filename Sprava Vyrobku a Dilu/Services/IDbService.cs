using Sprava_Vyrobku_a_Dilu.Database.Models;
using Sprava_Vyrobku_a_Dilu.Models;

namespace Sprava_Vyrobku_a_Dilu.Services
{
    public interface IDbService
    {
        Task<bool> AddDilModelAsync(DilModel dilModel);
        Task<bool> AddVyrobekModelAsync(Database.Models.VyrobekModel vyrobekModel);
        Task<bool> AnyDilyAsync();
        Task<bool> AnyVyrobkyAsync();
        Task<bool> DeleteDilModelAsync(int id);
        Task<bool> DeleteVyrobekModelAsync(int id);
        Task<IEnumerable<DilModel>> GetAllDilyAsync();
        Task<List<VyrobekModel>> GetAllVyrobkyAsync();
        Task<int> GetCountOfDilyByVyrobekIdAsync(int vyrobekId);
        Task<DilModel?> GetDilModelByIdAsync(int id);
        Task<Database.Models.VyrobekModel?> GetVyrobekModelByIdAsync(int id);
        Task<bool> UpdateDilModelAsync(DilModel dilModel);
        Task<bool> UpdateVyrobekModelAsync(Database.Models.VyrobekModel vyrobekModel);
        Task<bool> AddVyrobekWithDilyAsync(VyrobekModel vyrobekModel, IEnumerable<DilModel> dilModels);
    }
}