﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sprava_Vyrobku_a_Dilu.Database;
using Sprava_Vyrobku_a_Dilu.Database.Models;
using Sprava_Vyrobku_a_Dilu.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
                context.Dily.Add(dilModel);
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
                var existingDilModel = await context.Dily.FindAsync(dilModel.DilId);
                if (existingDilModel == null)
                    return false;

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

        public async Task<List<VyrobekViewModel>> GetAllVyrobkyAsync()
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                if (!await AnyVyrobkyAsync())
                {
                    return new List<VyrobekViewModel>();
                }

                return _mapper.Map<List<VyrobekViewModel>>(await context.Vyrobky.ToListAsync());
            }
            catch (Exception ex)
            {
                ShowError("GetAllVyrobkyAsync", ex);
                return new List<VyrobekViewModel>();
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

        public async Task<bool> DeleteVyrobekModelAsync(int id)
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
            MessageBox.Show($"Exception occurred in {methodName}: {ex.Message}\n{ex.StackTrace}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}