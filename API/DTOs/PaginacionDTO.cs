namespace FutbolAPI.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int cantidadMaximaRecordsPorPagina = 50;

        public int RecordsPorPagina { 
            get { return recordsPorPagina; }
            
            set {
                recordsPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value; } 
                }    //operador ternario verifica si es verdadero y le asigna el valor segun la verificacion.
                    // es decir si quiere poner un valor mayor a 50 este no lo dejara y pondra el valor maximo como predeterminado. 
    }
}
