using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; } = null!;

        private int currentCount = 0;


        public async void IncrementCount()
        {
            var authenticationState = await authenticationStateTask;
            var usuarioEstaAutenticado = authenticationState.User.Identity!.IsAuthenticated;

            if (usuarioEstaAutenticado)
            {
                currentCount++;
            }
            else
            {
                currentCount -= 1;
            }

           
            Console.WriteLine("Hola");
        }
    }
}
