using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprava_Vyrobku_a_Dilu.Database.Models
{
    public class VyrobekModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VyrobekId { get; set; }
        [Required]
        public string Nazev { get; set; }
        public string? Popis { get; set; }
        [Required]
        public decimal Cena { get; set; }
        public string? Poznamka { get; set; }
        public DateTime Zalozeno { get; init; } = DateTime.Now;
        public DateTime? Upraveno { get; set; }
        public ICollection<DilModel> Dily {get;} = new List<DilModel>();

        public VyrobekModel(string nazev, decimal cena)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(nazev));
            ArgumentNullException.ThrowIfNull(nameof(cena));
            Nazev = nazev;
            Cena = cena;
        }
    }
}
