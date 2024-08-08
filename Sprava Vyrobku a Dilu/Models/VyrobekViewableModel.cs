using PropertyChanged;
using SpravaVyrobkuaDilu.Database.Models;

namespace SpravaVyrobkuaDilu.Models
{
    [AddINotifyPropertyChangedInterface]
    public class VyrobekViewableModel
    {
        public int VyrobekId { get; set; }
        public string Nazev { get; set; } = string.Empty;
        public string? Popis { get; set; }
        public decimal Cena { get; set; }
        public string? Poznamka { get; set; }
        public DateTime Zalozeno { get; set; }
        public DateTime? Upraveno { get; set; }
        public int CountOfDily => Dily.Count;
        public List<DilModel> Dily { get; set; } = new List<DilModel>();
    }
}
