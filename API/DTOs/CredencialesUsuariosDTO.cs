using System.ComponentModel.DataAnnotations;

namespace FutbolAPI.DTOs
{
    public class CredencialesUsuariosDTO
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
