using System.ComponentModel.DataAnnotations;

namespace FutbolAPI.Entidades
{
    public class Equipo
    {

        public int id { get; set; }
        [Required (ErrorMessage = "El campo {0} es requerido")]
        [StringLength (20, ErrorMessage = "El campo {0} debe tener {1} o menos" )]
        public required string NombreEquipo { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(20, ErrorMessage = "El campo {0} debe tener {1} o menos")]
        public required string Responsable { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string Email { get; set; }

    }
}
