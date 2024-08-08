using System.Windows;
using Microsoft.EntityFrameworkCore;
using SpravaVyrobkuaDilu.Interfaces;
using SpravaVyrobkuaDilu.Database;
using SpravaVyrobkuaDilu.Database.Models;

namespace SpravaVyrobkuaDilu.Services
{
    public class DbService : IDbService
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

        public DbService(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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

        #endregion

        #region VyrobekModel Operations
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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

        #endregion
        /// <inheritdoc/>
        private static void ShowError(string methodName, Exception ex)
        {
            MessageBox.Show($"Exception occurred in {methodName}: {ex.Message},{ex?.InnerException?.Message} \n{ex?.StackTrace}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}