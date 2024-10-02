using BlazorPeliculas.Shared.Entidades;
using Microsoft.JSInterop;

namespace BlazorPeliculas.Client.Helpers
{
    public static class IJSRuntimeExtensionMethods
    {
        public static async ValueTask<bool> Confirm(this IJSRuntime js, string mensaje)
        {
            await js.InvokeVoidAsync("console.logo", "Prueba de Console log");
            return await js.InvokeAsync<bool>("confirm", mensaje);
        }
    }
}
 