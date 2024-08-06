using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PropertyChanged;
using Sprava_Vyrobku_a_Dilu.Database.Models;
using Sprava_Vyrobku_a_Dilu.Services;

namespace Sprava_Vyrobku_a_Dilu.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ObservableDataModel
    {
        private readonly IDbService _dbService;
        private readonly IMapper _mapper;

        public ObservableCollection<VyrobekViewableModel> ViewableVyrobky { get; set; }
        public ObservableCollection<VyrobekModel> Vyrobky { get; set; }
        public ObservableCollection<DilModel> Dily { get; set; }

        public bool IsLoading { get; set; } = false;

        public ObservableDataModel(IDbService dbService, IMapper mapper)
        {
            _dbService = dbService;
            _mapper = mapper;

            Vyrobky = new ObservableCollection<VyrobekModel>();
            Dily = new ObservableCollection<DilModel>();
            ViewableVyrobky = new ObservableCollection<VyrobekViewableModel>();

            // Subscribe to CollectionChanged events
            Vyrobky.CollectionChanged += OnVyrobkyChanged;
            Dily.CollectionChanged += OnDilyChanged;
        }

        public bool IsPresent(string name)
        {
            return Vyrobky.Any(v => v.Nazev.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task Create(VyrobekModel vyr,IEnumerable<DilModel> dilList )
        {
            if (await _dbService.AddVyrobekWithDilyAsync(vyr, dilList))
            {
                IsLoading = true;
                Vyrobky.Add(vyr);
                var viewableItem = _mapper.Map<VyrobekViewableModel>(vyr);
                ViewableVyrobky.Add(viewableItem);
                foreach (DilModel dil in dilList) 
                {
                    Dily.Add(dil);
                }
                IsLoading = false;
            }
        }

        private async void OnVyrobkyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsLoading) return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (VyrobekModel newItem in e.NewItems)
                {
                    if (await _dbService.AddVyrobekModelAsync(newItem))
                    {
                        var viewableItem = _mapper.Map<VyrobekViewableModel>(newItem);
                        ViewableVyrobky.Add(viewableItem);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (VyrobekModel oldItem in e.OldItems)
                {
                    if (await _dbService.DeleteVyrobekModelAsync(oldItem.VyrobekId))
                    {
                        var viewableItem = _mapper.Map<VyrobekViewableModel>(oldItem);
                        ViewableVyrobky.Remove(viewableItem);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (VyrobekModel newItem in e.NewItems)
                {
                    if (await _dbService.UpdateVyrobekModelAsync(newItem))
                    {
                        var viewableItem = _mapper.Map<VyrobekViewableModel>(newItem);
                    }
                }

                foreach (VyrobekModel oldItem in e.OldItems)
                {
                    if (await _dbService.DeleteVyrobekModelAsync(oldItem.VyrobekId))
                    {
                        var viewableItem = _mapper.Map<VyrobekViewableModel>(oldItem);
                        ViewableVyrobky.Remove(viewableItem);
                    }
                }
            }
        }

        private async void OnDilyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsLoading) return;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (DilModel newItem in e.NewItems)
                {
                    await _dbService.AddDilModelAsync(newItem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (DilModel oldItem in e.OldItems)
                {
                    await _dbService.DeleteDilModelAsync(oldItem.DilId);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (DilModel newItem in e.NewItems)
                {
                    await _dbService.UpdateDilModelAsync(newItem);
                }
                foreach (DilModel oldItem in e.OldItems)
                {
                    await _dbService.DeleteDilModelAsync(oldItem.DilId);
                }
            }
        }
    }
}
