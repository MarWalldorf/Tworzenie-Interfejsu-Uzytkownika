using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabTiu7.Models
{
    public class Laptop
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Producent { get; set; } = string.Empty; // Pole tekstowe

        [Required]
        [MaxLength(150)]
        public string Model { get; set; } = string.Empty; // Pole tekstowe

        public int PamiecRamGB { get; set; } // Pole liczbowe całkowite

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Cena { get; set; } // Pole liczbowe zmiennoprzecinkowe

        public bool CzyDostepny { get; set; } // Pole boolowskie
    }
}