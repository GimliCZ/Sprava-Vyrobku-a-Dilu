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

        /// <summary>
        /// Gets or sets the collection of viewable product models.
        /// </summary>
        public ObservableCollection<VyrobekViewableModel> ViewableVyrobky { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableDataProvider"/> class.
        /// </summary>
        /// <param name="dbService">The database service used for data operations.</param>
        /// <param name="mapper">The mapper used for model transformations.</param>
        public ObservableDataProvider(IDbService dbService, IMapper mapper)
        {
            _dbService = dbService;
            _mapper = mapper;

            ViewableVyrobky = new ObservableCollection<VyrobekViewableModel>();
        }

        /// <summary>
        /// Determines whether a vyrobek with the specified name is present in the collection.
        /// </summary>
        /// <param name="name">The name of the product.</param>
        /// <returns><c>true</c> if the product is present; otherwise, <c>false</c>.</returns>
        public bool IsPresent(string name)
        {
            return ViewableVyrobky.Any(v => v.Nazev.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// Adds a new vyrobek and its associated dil to the database and updates the collection.
        /// </summary>
        /// <param name="vyr">The vyrobek model to add.</param>
        /// <param name="dilList">The list of dil to associate with the product.</param>
        /// <returns><c>true</c> if the addition was successful; otherwise, <c>false</c>.</returns>
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
        /// <summary>
        /// Removes a vyrobek from the database and updates the collection.
        /// </summary>
        /// <param name="vyrobek">The Vyrobek to remove.</param>
        /// <returns><c>true</c> if the removal was successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> RemoveVyrobek(VyrobekViewableModel vyrobek)
        {
            if (await _dbService.DeleteVyrobekModelByIdAsync(vyrobek.VyrobekId))
            {
                ViewableVyrobky.Remove(vyrobek);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Updates an existing vyrobek in the database and reflects changes in the collection.
        /// </summary>
        /// <param name="vyrobek">The vyrobek model to update.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> UpdateVyrobek(VyrobekModel vyrobek)
        {

            var vyrobekToUpgrade = ViewableVyrobky.Where(p => p.VyrobekId == vyrobek.VyrobekId).SingleOrDefault();

            if (vyrobekToUpgrade is not null and VyrobekViewableModel)
            {
                if (await _dbService.UpdateVyrobekModelAsync(vyrobek))
                {
                    vyrobek.Zalozeno = vyrobekToUpgrade.Zalozeno;
                    ViewableVyrobky.Remove(vyrobekToUpgrade);
                    var updatedVyrobek = _mapper.Map<VyrobekViewableModel>(vyrobek);
                    updatedVyrobek.Dily = vyrobekToUpgrade.Dily;
                    ViewableVyrobky.Add(updatedVyrobek);
                    return true;
                }
            }
            return false;
        }
        //HACK:
        //Observable collection does not detect nested changes.
        //In order to trigger change notify, we must do swap
        //Ideally this should change to nested change notification pass
        //But this will suffice

        /// <summary>
        /// Adds a new dil to an existing product in the database and updates the collection.
        /// </summary>
        /// <param name="dil">The dil model to add.</param>
        /// <returns><c>true</c> if the addition was successful; otherwise, <c>false</c>.</returns>
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
        /// <summary>
        /// Updates an existing dil in the database and reflects changes in the collection.
        /// </summary>
        /// <param name="dil">The dil model to update.</param>
        /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
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
        /// <summary>
        /// Removes a dil from an existing product in the database and updates the collection.
        /// </summary>
        /// <param name="dil">The dil model to remove.</param>
        /// <returns><c>true</c> if the removal was successful; otherwise, <c>false</c>.</returns>
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
        /// <summary>
        /// Refreshes the collection by retrieving all products and parts from the database.
        /// </summary>
        public async Task Refresh()
        {
            var vyrobky = await _dbService.GetAllVyrobkyAsync();
            ViewableVyrobky.Clear();

            foreach (var vyrobekModel in vyrobky)
            {
                var viewableModel = _mapper.Map<VyrobekViewableModel>(vyrobekModel);
                ViewableVyrobky.Add(viewableModel);
            }
        }
    }
}
