using Ecommerce.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOs
{
    public class UserDTO : User
    {

        [DataType(DataType.Password)]
        [Display(Name = "Contrasena")]
        [Required(ErrorMessage = "El campo {0} Es Requerido")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener {2} y {1} Caracteres")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "La contrasena de confirmacion debe se igual")]
        [Display(Name = "Confirmacion Contrasena")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} Es Requerido")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe tener {2} y {1} Caracteres")]

        public string PasswordConfirm { get; set; } = null!;

    }
}
