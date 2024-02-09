using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unna.OperationalReport.Service.Auth.Dtos
{
    public class LoginPeticionDto
    {
        [Required(ErrorMessage = "Correo es requerido")]        
        public string? Username { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida")]        
        public string? Password { get; set; }

        
    }
}
