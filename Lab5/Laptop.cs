using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiuLab5.Models
{
    [Table("Laptopy")]
    public class Laptop
    {
        [Key]
        public int LaptopId { get; set; }

        [Required(ErrorMessage = "Nazwa modelu jest wymagana.")]
        [StringLength(20, ErrorMessage = "Nazwa modelu nie może przekraczać 20 znaków.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-zA-Z0-9ąęćłńóśźżĄĆĘŁŃÓŚŹŻ\s\-]*$",
            ErrorMessage = "Nazwa musi zaczynać się wielką literą i zawierać tylko litery, cyfry, spacje lub myślnik.")]
        [Display(Name = "Model laptopa")]
        public string Model { get; set; }

        [Range(1.0, 5.0, ErrorMessage = "Ocena musi mieścić się w przedziale od 1.0 do 5.0.")]
        [Display(Name = "Ocena (opcjonalna)")]
        public double? Ocena { get; set; }

        [Display(Name = "Dostępny w magazynie")]
        public bool Dostepny { get; set; }

        // Klucz obcy (opcjonalny -> krotność 0..1)
        [Display(Name = "Podzespół")]
        public int? PodzespolId { get; set; }

        // Właściwość nawigacyjna
        [ForeignKey("PodzespolId")]
        [Display(Name = "Podzespół")]
        public virtual Podzespol? Podzespol { get; set; }
    }
}