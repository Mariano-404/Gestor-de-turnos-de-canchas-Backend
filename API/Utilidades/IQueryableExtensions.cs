using FutbolAPI.DTOs;

namespace FutbolAPI.Utilidades
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacion)
        {
            return queryable
                .Skip((paginacion.Pagina - 1) * paginacion.RecordsPorPagina)//esta es una formula es si esta en la pagina 1 se salta porque 1*1 es 1 y -1 da 0 entonces salta 0 paginas
                .Take(paginacion.RecordsPorPagina);
        }
    }
}
