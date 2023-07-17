using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Aplicacion;
using API.Dominio;

namespace API.Aplicacion.DTOsUser
{
    public class UsuarioLoginRespuestaVM
    {
        public string Token { get; set; }
        public Usuario Usuario { get; set; }
    }
}
