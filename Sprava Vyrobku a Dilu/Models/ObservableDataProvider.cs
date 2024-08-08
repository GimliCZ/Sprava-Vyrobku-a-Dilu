using System.Collections.ObjectModel;
using AutoMapper;
using PropertyChanged;
using SpravaVyrobkuaDilu.Database.Models;
using SpravaVyrobkuaDilu.Services;

namespace SpravaVyrobkuaDilu.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ObservableDataProvider
    {
        private readonly IDbService _dbService;
        private readonly IMapper _mapper;

        public ObservableCollection<VyrobekViewableModel> ViewableVyrobky { get; set; }
        public List<VyrobekModel> Vyrobky { get; set; }

        public bool IsLoading { get; set; } = false;

        public ObservableDataProvider(IDbService dbService, IMapper mapper)
        {
            _dbService = dbService;
            _mapper = mapper;

            Vyrobky = new List<VyrobekModel>();
            ViewableVyrobky = new ObservableCollection<VyrobekViewableModel>();
        }

        public bool IsPresent(string name)
        {
            return Vyrobky.Any(v => v.Nazev.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        //TODO: Předělat
        public async Task<bool> AddVyrobekAndDily(VyrobekModel vyr, IEnumerable<DilModel> dilList)
        {
            if (await _dbService.AddVyrobekWithDilyAsync(vyr, dilList))
            {
                var viewableItem = _mapper.Map<VyrobekViewableModel>(vyr);
                ViewableVyrobky.Add(viewableItem);
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveVyrobek(VyrobekViewableModel vyrobek)
        {
            if (await _dbService.DeleteVyrobekModelByIdAsync(vyrobek.VyrobekId))
            {
                ViewableVyrobky.Remove(vyrobek);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateVyrobek(VyrobekModel vyrobek)
        {

            var vyrobekToUpgrade = ViewableVyrobky.Where(p => p.VyrobekId == vyrobek.VyrobekId).SingleOrDefault();

            if (vyrobekToUpgrade != null && vyrobekToUpgrade is VyrobekViewableModel)
            {
                if (await _dbService.UpdateVyrobekModelAsync(vyrobek))
                {
                    vyrobek.Zalozeno = vyrobekToUpgrade.Zalozeno;
                    ViewableVyrobky.Remove(vyrobekToUpgrade);
                    ViewableVyrobky.Add(_mapper.Map<VyrobekViewableModel>(vyrobek));
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> AddDil(DilModel dil)
        {
            if (!ViewableVyrobky.Any(x => x.VyrobekId == dil.VyrobekId))
            {
                return false;
            }

            if (await _dbService.AddDilModelAsync(dil))
            {
                var model = ViewableVyrobky.Where(x => x.VyrobekId == dil.VyrobekId).SingleOrDefault();
                if (model != null)
                {
                    ViewableVyrobky.Remove(model);
                    model.Dily.Add(dil);
                    ViewableVyrobky.Add(model);
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> UpdateDil(DilModel dil)
        {
            if (!ViewableVyrobky.Any(x => x.VyrobekId == dil.VyrobekId))
            {
                return false;
            }

            if (await _dbService.UpdateDilModelAsync(dil))
            {
                var model = ViewableVyrobky.Where(x => x.VyrobekId == dil.VyrobekId).SingleOrDefault();
                if (model != null)
                {
                    var oldDil = model.Dily.Where(x => x.DilId == dil.DilId).SingleOrDefault();
                    if (oldDil != null)
                    {
                        dil.Zalozeno = oldDil.Zalozeno;
                        ViewableVyrobky.Remove(model);
                        model.Dily.Remove(oldDil);
                        model.Dily.Add(dil);
                        ViewableVyrobky.Add(model);
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> RemoveDil(DilModel dil)
        {
            if (!ViewableVyrobky.Any(x => x.VyrobekId == dil.VyrobekId))
            {
                return false;
            }

            if (await _dbService.DeleteDilModelAsync(dil.DilId))
            {
                var model = ViewableVyrobky.Where(x => x.VyrobekId == dil.VyrobekId).SingleOrDefault();
                if (model != null)
                {
                    ViewableVyrobky.Remove(model);
                    model.Dily.Remove(dil);
                    ViewableVyrobky.Add(model);
                    return true;
                }
            }
            return false;
        }

        public async Task Refresh()
        {
            var vyrobky = await _dbService.GetAllVyrobkyAsync();
            var dily = await _dbService.GetAllDilyAsync();
            Vyrobky.Clear();
            ViewableVyrobky.Clear();

            foreach (var vyrobekModel in vyrobky)
            {
                Vyrobky.Add(vyrobekModel);
                var viewableModel = _mapper.Map<VyrobekViewableModel>(vyrobekModel);
                ViewableVyrobky.Add(viewableModel);
            }
        }
    }
}
