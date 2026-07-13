using System.ComponentModel.DataAnnotations;

namespace FutbolAPI.DTOs
{
    public class FechaYhoraCeacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string FechaCancha { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string HoraCancha { get; set; }
        public int EquipoId { get; set; }
        public int CanchaId { get; set; }
    }
}
