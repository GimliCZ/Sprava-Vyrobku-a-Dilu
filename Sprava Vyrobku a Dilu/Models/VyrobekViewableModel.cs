using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprava_Vyrobku_a_Dilu.Models
{
    public class VyrobekViewableModel
    {
        public int VyrobekId { get; set; }
        public string Nazev { get; set; }
        public string? Popis { get; set; }
        public decimal Cena { get; set; }
        public string? Poznamka { get; set; }
        public DateTime Zalozeno { get; set; }
        public DateTime? Upraveno { get; set; }

        // Property to hold the count of Dily items
        public int CountOfDily { get; set; }
    }
}
