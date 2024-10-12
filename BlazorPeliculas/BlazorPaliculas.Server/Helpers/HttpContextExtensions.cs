using Microsoft.EntityFrameworkCore;

namespace BlazorPaliculas.Server.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnRespuesta<T>(
            this HttpContext context, IQueryable<T> queryable, int cantidadRegistrosAMostrar)
        { 
            if(context is null) 
            { 
                throw new ArgumentNullException(nameof(context));
            }

            double conteo = await queryable.CountAsync();
            double totalPaginas = Math.Ceiling(conteo / cantidadRegistrosAMostrar);
            context.Response.Headers.Append("conteo", conteo.ToString());
            context.Response.Headers.Append("totalPaginas", totalPaginas.ToString());
        }
    }
}
