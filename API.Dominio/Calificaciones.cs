﻿using Npgsql.Replication.PgOutput;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage ="Ingrese el Periodo por favor")]
        public int PeriodoId { get; set; }
        public PeriodoEscolar Periodo { get; set; }
        [Required]
        public int MateriaId { get; set; }


        public int? Unidad1 { get; set; } = null;
        public int? Unidad2 { get; set; } = null;
        public int? Unidad3 { get; set; } = null;
        public int? Unidad4 { get; set; } = null;
        public int? Unidad5 { get; set; } = null;
        public decimal? CalificacionFinal { get; set; } = null;

    }
}
