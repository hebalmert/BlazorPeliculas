using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorPeliculas.Client.Auth
{
    public class ProveedorAutorizacionPrueba : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimo = new ClaimsIdentity();
            var usuarioHebert = new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim("edad", "999"),
                    new Claim(ClaimTypes.Name, "Hebert M"),
                    new Claim(ClaimTypes.Role, "aux")
                },
                authenticationType: "prueba");

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(usuarioHebert)));
        }
    }
}
