using System.ComponentModel.DataAnnotations;

namespace FutbolAPI.Entidades
{
    public class Cancha
    {

        public int id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string NumeroCancha { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string TipoCancha { get; set; }
    }
}
