using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using Sprava_Vyrobku_a_Dilu.Database.Models;

namespace Sprava_Vyrobku_a_Dilu.Models
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
        public List<DilModel> Dily { get; set; } = new List<DilModel> ();
    }
}
