using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.DTO
{
    public class VillaUpdateDTO
    {
        [Required]
        public int id { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }

        [Required]
        public int Ocupantes { get; set; }

        [Required]
        public int MetrosCuadrados { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public string Amenidad { get; set; }

        [Required]
        public string ImagenUrl { get; set; }
    }
}
