using System.Windows;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sprava_Vyrobku_a_Dilu.Database;
using Sprava_Vyrobku_a_Dilu.Database.Models;

namespace Sprava_Vyrobku_a_Dilu.Services
{
    public class DbService : IDbService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public DbService(IDbContextFactory<AppDbContext> dbContextFactory,
            IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public async Task<bool> AddVyrobekWithDilyAsync(VyrobekModel vyrobekModel, IEnumerable<DilModel> dilModels)
        {
            ArgumentNullException.ThrowIfNull(vyrobekModel);
            ArgumentNullException.ThrowIfNull(dilModels);

            try
            {
                using var context = _dbContextFactory.CreateDbContext();

                // Add the VyrobekModel to the context
                await context.Vyrobky.AddAsync(vyrobekModel);
                await context.SaveChangesAsync(); // Save changes to get the VyrobekId

                // Now set the VyrobekId for each DilModel and add them to the context
                foreach (var dilModel in dilModels)
                {
                    dilModel.VyrobekId = vyrobekModel.VyrobekId; // Set the foreign key
                }

                await context.Dily.AddRangeAsync(dilModels);
                var changes = await context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                ShowError("AddVyrobekWithDilyAsync", ex);
                return false;
            }
        }

        #region DilModel Operations

        public async Task<bool> AnyDilyAsync()
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                return await context.Dily.AnyAsync();
            }
            catch (Exception ex)
            {
                ShowError("AnyDilyAsync", ex);
                return false;
            }
        }

        public async Task<bool> AddDilModelAsync(DilModel dilModel)
        {
            ArgumentNullException.ThrowIfNull(dilModel);

            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                await context.Dily.AddAsync(dilModel);
                var changes = await context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                ShowError("AddDilModelAsync", ex);
                return false;
            }
        }

        public async Task<DilModel?> GetDilModelByIdAsync(int id)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                return await context.Dily
                    .Include(d => d.Vyrobek) // Include related VyrobekModel
                    .FirstOrDefaultAsync(d => d.DilId == id);
            }
            catch (Exception ex)
            {
                ShowError("GetDilModelByIdAsync", ex);
                return null;
            }
        }

        public async Task<IEnumerable<DilModel>> GetAllDilyAsync()
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                if (!await AnyDilyAsync())
                {
                    return Enumerable.Empty<DilModel>();
                }

                return await context.Dily.ToListAsync();
            }
            catch (Exception ex)
            {
                ShowError("GetAllDilyAsync", ex);
                return Enumerable.Empty<DilModel>();
            }
        }

        public async Task<bool> UpdateDilModelAsync(DilModel dilModel)
        {
            ArgumentNullException.ThrowIfNull(dilModel);

            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                var existingDilModel = context.Dily.Where(x => x.DilId == dilModel.DilId).SingleOrDefault();
                if (existingDilModel == null)
                    return false;
                dilModel.Zalozeno = existingDilModel.Zalozeno;
                context.Entry(existingDilModel).CurrentValues.SetValues(dilModel);
                var changes = await context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                ShowError("UpdateDilModelAsync", ex);
                return false;
            }
        }

        public async Task<bool> DeleteDilModelAsync(int id)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                var dilModel = await context.Dily.FindAsync(id);
                if (dilModel == null)
                    return false;

                context.Dily.Remove(dilModel);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ShowError("DeleteDilModelAsync", ex);
                return false;
            }
        }

        public async Task<bool> DeleteDilByVyrobekModelAsync(int idVyrobek)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                var dilModel = await context.Dily.Where(x => x.VyrobekId == idVyrobek).ToListAsync();
                if (dilModel == null)
                    return false;

                context.Dily.RemoveRange(dilModel);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ShowError("DeleteDilModelAsync", ex);
                return false;
            }
        }

        #endregion

        #region VyrobekModel Operations

        public async Task<bool> AnyVyrobkyAsync()
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                return await context.Vyrobky.AnyAsync();
            }
            catch (Exception ex)
            {
                ShowError("AnyVyrobkyAsync", ex);
                return false;
            }
        }

        public async Task<bool> AddVyrobekModelAsync(VyrobekModel vyrobekModel)
        {
            ArgumentNullException.ThrowIfNull(vyrobekModel);

            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                context.Vyrobky.Add(vyrobekModel);
                var changes = await context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                ShowError("AddVyrobekModelAsync", ex);
                return false;
            }
        }

        public async Task<VyrobekModel?> GetVyrobekModelByIdAsync(int id)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                return await context.Vyrobky
                    .Include(v => v.Dily) // Include related DilModel
                    .FirstOrDefaultAsync(v => v.VyrobekId == id);
            }
            catch (Exception ex)
            {
                ShowError("GetVyrobekModelByIdAsync", ex);
                return null;
            }
        }

        public async Task<List<VyrobekModel>> GetAllVyrobkyAsync()
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                if (!await AnyVyrobkyAsync())
                {
                    return new List<VyrobekModel>();
                }
                var data = await context.Vyrobky.Include(x => x.Dily).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                ShowError("GetAllVyrobkyAsync", ex);
                return new List<VyrobekModel>();
            }
        }

        public async Task<bool> UpdateVyrobekModelAsync(VyrobekModel vyrobekModel)
        {
            ArgumentNullException.ThrowIfNull(vyrobekModel);

            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                var existingVyrobekModel = await context.Vyrobky.FindAsync(vyrobekModel.VyrobekId);
                if (existingVyrobekModel == null)
                    return false;
                vyrobekModel.Zalozeno = existingVyrobekModel.Zalozeno;
                context.Entry(existingVyrobekModel).CurrentValues.SetValues(vyrobekModel);
                var changes = await context.SaveChangesAsync();
                return changes > 0;
            }
            catch (Exception ex)
            {
                ShowError("UpdateVyrobekModelAsync", ex);
                return false;
            }
        }

        public async Task<bool> DeleteVyrobekModelByIdAsync(int id)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                var vyrobekModel = await context.Vyrobky.FindAsync(id);
                if (vyrobekModel == null)
                    return false;

                context.Vyrobky.Remove(vyrobekModel);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                ShowError("DeleteVyrobekModelAsync", ex);
                return false;
            }
        }

        public async Task<int> GetCountOfDilyByVyrobekIdAsync(int vyrobekId)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                return await context.Dily.CountAsync(d => d.VyrobekId == vyrobekId);
            }
            catch (Exception ex)
            {
                ShowError("GetCountOfDilyByVyrobekIdAsync", ex);
                return 0;
            }
        }

        #endregion

        private void ShowError(string methodName, Exception ex)
        {
            MessageBox.Show($"Exception occurred in {methodName}: {ex.Message},{ex?.InnerException?.Message} \n{ex.StackTrace}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}