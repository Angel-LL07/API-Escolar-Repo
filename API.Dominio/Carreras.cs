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
        [Required(ErrorMessage = "El Nombre de la carrera es obligatorio")]
        public string NombreCarrera { get; set; }
        [Required(ErrorMessage = "Las siglas de la carrera son obligatorias")]
        public string Siglas { get; set; }
        [Required(ErrorMessage = "El Nombre reducido es obligatorio")]
        public string NombreReducido { get; set; }
        [Required(ErrorMessage = "La reticula es obligatoria")]
        public int Reticula { get; set; }
    }
}
