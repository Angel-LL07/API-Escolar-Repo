using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio
{
    public class Materias
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre de la materia es necesario")]
        public string NombreMateria { get; set; }
        [Required(ErrorMessage = "La clave es necesaria")]
        public string Clave { get; set; }
        [Required]
        public int CarreraId { get; set; }
        public Carreras Carrera { get; set; }
        [Required]
        public int NoUnidades { get; set; }
    }
}
