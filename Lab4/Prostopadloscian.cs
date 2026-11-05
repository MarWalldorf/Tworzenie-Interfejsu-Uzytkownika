using System.ComponentModel.DataAnnotations;

namespace Lab4_Sekcja1.Models
{
    public class Prostopadloscian
    {
        [Required(ErrorMessage = "Długość jest wymagana.")]
        [Range(1, int.MaxValue, ErrorMessage = "Długość musi być liczbą dodatnią.")]
        [DataType(DataType.Text)]
        [RegularExpression(@"^\d*[02468]$", ErrorMessage = "Długość musi być całkowitą liczbą parzystą.")]
        public int Dlugosc { get; set; }

        [Required(ErrorMessage = "Szerokość jest wymagana.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Szerokość musi być liczbą dodatnią.")]
        public double Szerokosc { get; set; }

        [Required(ErrorMessage = "Wysokość jest wymagana.")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Wysokość musi być liczbą dodatnią.")]
        public double Wysokosc { get; set; }

        public double Objetosc { get; private set; }
        public double Pole { get; private set; }

        public Prostopadloscian(int dlugosc, double szerokosc, double wysokosc)
        {
            Dlugosc = dlugosc;
            Szerokosc = szerokosc;
            Wysokosc = wysokosc;
        }

        public Prostopadloscian()
        {
        }

        public void Oblicz()
        {
            Objetosc = Dlugosc * Szerokosc * Wysokosc;
            Pole = 2 * (Dlugosc * Szerokosc + Dlugosc * Wysokosc + Szerokosc * Wysokosc);
        }
    }
}