using System.ComponentModel.DataAnnotations;

namespace FutbolAPI.Entidades
{
    public class FechaYhora
    {
        public int id {  get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string FechaCancha { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public required string HoraCancha { get; set; }
        public int EquipoId { get; set; }
        public Equipo Equipo { get; set; } = null!;
        public int? EquipoRivalId { get; set; }  // lo hago nullable para que inicialmente pueda ser null
        public Equipo? EquipoRival { get; set; }
        public int CanchaId { get; set; }
        public Cancha Cancha { get; set; } = null!;
    }
}
