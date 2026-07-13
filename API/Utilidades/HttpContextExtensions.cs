using Microsoft.EntityFrameworkCore;

namespace FutbolAPI.Utilidades
{
    public static class HttpContextExtensions
    {
        //este agrega al metodo httpcontext uno nuevo que son los que agregegos a esta clase.
        public async static Task InsertarParametrosPaginacion<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if(httpContext is null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double cantidad = await queryable.CountAsync();
            httpContext.Response.Headers.Append("cantidad-total-registros", cantidad.ToString());
        }
    }
}
