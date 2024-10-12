using System.ComponentModel.DataAnnotations;

namespace BlazorPeliculas.Shared.DTOs
{
    public class UserInfoDTO
    {
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
