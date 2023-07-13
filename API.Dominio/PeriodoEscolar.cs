using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio
{
    public class PeriodoEscolar
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La descripción del periodo es necesaria")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La abreviación del periodo es necesaria")]
        public string Abreviacion { get; set; }
        [Required(ErrorMessage = "El estatus del periodo es necesario")]
        public string Status { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTemino { get; set; }

    }
}
