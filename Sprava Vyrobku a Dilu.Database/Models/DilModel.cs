using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpravaVyrobkuaDilu.Database.Models
{
    public class DilModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DilId { get; set; }
        [Required]
        public string Nazev { get; set; }
        public string? Popis { get; set; }
        [Required]
        public decimal Cena { get; set; }
        public DateTime Zalozeno { get; set; } = DateTime.Now;
        public DateTime? Upraveno { get; set; }
        [Required]
        public int VyrobekId { get; set; }
        [ForeignKey(nameof(VyrobekId))]
        public VyrobekModel Vyrobek { get; set; } = null!;

        public DilModel(string nazev, decimal cena, int vyrobekId)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(nazev));
            ArgumentNullException.ThrowIfNull(nameof(cena));
            ArgumentNullException.ThrowIfNull(nameof(vyrobekId));
            Nazev = nazev;
            Cena = cena;
            VyrobekId = vyrobekId;
        }
    }
}