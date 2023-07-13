using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dominio
{
    public class Estudiantes
    {
        [Key]
        public int NoControl { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_Materno { get; set; }
        public string Nombre { get; set; }
        public  int Edad { get; set; }
        public string Curp { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public char Sexo { get; set; }
        public int CarreraId { get; set; }
        public Carreras Carrera { get; set; }


    }
}
