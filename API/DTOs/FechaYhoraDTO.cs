namespace FutbolAPI.DTOs
{
    public class FechaYhoraDTO
    {
        public int id { get; set; }
        public required string FechaCancha { get; set; }
        public required string HoraCancha { get; set; }
        public int EquipoId { get; set; }
        public int CanchaId { get; set; }
        public int? EquipoRivalId { get; set; }
        public string TipoCancha { get; set; } = null!;
        public string NumeroCancha { get; set; } = null!;
        public string NombreEquipo { get; set; } = null!;
    }
}
