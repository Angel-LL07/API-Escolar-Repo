using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio
{
    public class PeriodoEscolar
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Abreviacion { get; set; }
        public string Status { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTemino { get; set; }

    }
}
