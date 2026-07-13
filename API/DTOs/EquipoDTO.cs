using System.ComponentModel.DataAnnotations;

namespace FutbolAPI.DTOs
{
    public class EquipoDTO
    {
        public int id { get; set; }
       
        public required string NombreEquipo { get; set; }
       
        public required string Responsable { get; set; }
        
        public required string Email { get; set; }
    }
}
