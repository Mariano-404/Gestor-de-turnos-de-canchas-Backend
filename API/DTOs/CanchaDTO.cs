using System.ComponentModel.DataAnnotations;

namespace FutbolAPI.DTOs
{
    public class CanchaDTO
    {
        public int id { get; set; }
        
        public required string NumeroCancha { get; set; }
        
        public required string TipoCancha { get; set; }
    }
}
