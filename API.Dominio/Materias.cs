using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio
{
    public class Materias
    {
        public int Id { get; set; }
        public string ClaveMateria { get; set; }
        public string NombreMateria { get; set; }
        public int CarreraId { get; set; }
        public Carreras Carrera { get; set; }
        public int NoUnidades { get; set; }
    }
}
