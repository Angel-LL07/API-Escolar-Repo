﻿
using System.ComponentModel.DataAnnotations;

namespace APIEscolar.DTOs
{
    public class MateriasVM

    {
        public int Id { get; set; }
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
