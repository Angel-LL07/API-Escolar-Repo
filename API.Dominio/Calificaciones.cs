using Npgsql.Replication.PgOutput;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio
{
    public class Calificaciones
    {
        public int id { get; set; }
        [ForeignKey("NoControl")]
        public int EstudiantesNoControl { get; set; }
        public Estudiantes Estudiantes { get; set; }
        public int PeriodoId { get; set; }
        public PeriodoEscolar Periodo { get; set; }


        public int Unidad1 { get; set; }
        public int Unidad2 { get; set; }
        public int Unidad3 { get; set; }
        public int Unidad4 { get; set; }
        public int Unidad5 { get; set; }
        public decimal CalificacionFinal { get; set; }

    }
}
