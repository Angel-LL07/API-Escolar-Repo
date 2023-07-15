using System.ComponentModel.DataAnnotations;

namespace APIEscolar.DTOs
{
    public class CarrerasCrearVM
    {
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
