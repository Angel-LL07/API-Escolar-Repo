using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Aplicacion.DTOsUser
{
    public class UsuarioVM
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese su nombre de Usuario")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Ingrese su Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese su Contraseña")]
        public string Contraseña { get; set; }
        [Required(ErrorMessage = "Ingrese el rol")]
        public string Role { get; set; }
    }
}
