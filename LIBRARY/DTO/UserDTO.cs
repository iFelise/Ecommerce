using LIBRARY.Shared.Entity;
using System.ComponentModel.DataAnnotations;

namespace LIBRARY.Shared.DTO
{
    public class UserDTO:User
    {
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6,
            ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmación de contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6,
            ErrorMessage = "El campo {0} debe tener entre {2} y {1} carácteres.")]
        public required string ConfirmPassword { get; set; }
        [Display(Name = "Foto de perfil")]
        public string? PhotoFile { get; set; }  // Para uploads directos de archivos

        [Display(Name = "URL de foto")]
        public string? PhotoUrl { get; set; }
    }
}
