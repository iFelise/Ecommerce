using System.ComponentModel.DataAnnotations;

namespace LIBRARY.Shared.DTO
{
    public class LoginDTO
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Correo electrónico")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo válido.")]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Contraseña")]
        [StringLength(20, MinimumLength = 6,
            ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public required string Password { get; set; }
    }
}
