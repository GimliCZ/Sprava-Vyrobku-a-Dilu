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
        public ObservableCollection<VyrobekModel> Vyrobky { get; set; }
        public ObservableCollection<DilModel> Dily { get; set; }

        public bool IsLoading { get; set; } = false;

        public ObservableDataModel(IDbService dbService, IMapper mapper)
        {
            _dbService = dbService;
            _mapper = mapper;

            Vyrobky = new ObservableCollection<VyrobekModel>();
            Dily = new ObservableCollection<DilModel>();

            // Subscribe to CollectionChanged events
            Vyrobky.CollectionChanged += OnVyrobkyChanged;
            Dily.CollectionChanged += OnDilyChanged;
        }

        public bool IsPresent(string name)
        {
            return Vyrobky.Any(v => v.Nazev.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private async void OnVyrobkyChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsLoading) return;  

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var newItems = e.NewItems.Cast<VyrobekModel>();
                foreach (var newItem in newItems)
                {
                    var vyrobekModel = _mapper.Map<VyrobekModel>(newItem);
                    var dbVyrobekModel =  await _dbService.AddVyrobekModelAsync(vyrobekModel);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var oldItems = e.OldItems.Cast<VyrobekModel>();
                foreach (var oldItem in oldItems)
                {
                    var vyrobekModel = _mapper.Map<VyrobekModel>(oldItem);
                    await _dbService.DeleteVyrobekModelAsync(vyrobekModel.VyrobekId);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var newItems = e.NewItems.Cast<VyrobekModel>();
                var oldItems = e.OldItems.Cast<VyrobekModel>();

                foreach (var newItem in newItems)
                {
                    var vyrobekModel = _mapper.Map<VyrobekModel>(newItem);
                    await _dbService.UpdateVyrobekModelAsync(vyrobekModel);
                }

                foreach (var oldItem in oldItems)
                {
                    var vyrobekModel = _mapper.Map<VyrobekModel>(oldItem);
                    await _dbService.DeleteVyrobekModelAsync(vyrobekModel.VyrobekId);
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
