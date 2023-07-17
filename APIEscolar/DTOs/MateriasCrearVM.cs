using API.Dominio;
using System.ComponentModel.DataAnnotations;

namespace APIEscolar.DTOs
{
    public class MateriasCrearVM
    {

        [Required(ErrorMessage = "El nombre de la materia es necesario")]
        public string NombreMateria { get; set; }
        [Required(ErrorMessage = "La clave es necesaria")]
        public string Clave { get; set; }
        [Required]
        public int CarreraId { get; set; }
        [Required]
        public int NoUnidades { get; set; }
    }
}
