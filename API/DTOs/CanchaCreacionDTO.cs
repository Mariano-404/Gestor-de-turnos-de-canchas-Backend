using System.ComponentModel.DataAnnotations;

namespace FutbolAPI.DTOs
{
    public class CanchaCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string NumeroCancha { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string TipoCancha { get; set; }
    }
}
