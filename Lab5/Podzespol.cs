using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiuLab5.Models
{
    [Table("Podzespoly")]
    public class Podzespol
    {
        [Key]
        public int PodzespolId { get; set; }

        [Required(ErrorMessage = "Nazwa podzespołu jest wymagana.")]
        [StringLength(50, ErrorMessage = "Nazwa nie może przekraczać 50 znaków.")]
        [Display(Name = "Podzespół")]
        public string Nazwa { get; set; }

        // Właściwość nawigacyjna kolekcji (0..n)
        public virtual ICollection<Laptop>? Laptopy { get; set; }
    }
}
