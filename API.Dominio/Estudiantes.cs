﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required(ErrorMessage ="Ingrese su Apellido Paterno")]
        public string Apellido_Paterno { get; set; }
        [Required(ErrorMessage = "Ingrese su Apellido Materno")]
        public string Apellido_Materno { get; set; }
        [Required(ErrorMessage = "Ingrese su Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese su Edad")]
        public  int Edad { get; set; }
        [RegularExpression("^([A-Z][AEIOUX][A-Z]{2}\\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\\d])(\\d)$", ErrorMessage = "curp no valida")]
        [Required]
        [StringLength(18, ErrorMessage = "la {0} debe contener una longitud de 18 caractares Y debe ser valida")]
        public string Curp { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        [Display(Name = "Sexo M o F")]
        public string Sexo { get; set; }
        [Required(ErrorMessage = "Ingrese el Id de la Carrera")]
        public int CarreraId { get; set; }
        public Carreras Carrera { get; set; }


    }
}
