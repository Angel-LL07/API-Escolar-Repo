
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APIEscolar.DTOs
{
    public class CalificacionesVM
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Ingrese el Numero de Control")]
        public int EstudiantesNoControl { get; set; }
        [Required(ErrorMessage = "Ingrese el Periodo por favor")]
        public int? PeriodoId { get; set; } = null;
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
