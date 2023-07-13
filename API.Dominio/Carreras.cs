using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio
{
    public class Carreras
    {
        [Key]
        public int Id { get; set; }
        public string NombreCarrera { get; set; }
        public string Siglas { get; set; }
        public string NombreReducido { get; set; }
        public string Reticula { get; set; }
    }
}
