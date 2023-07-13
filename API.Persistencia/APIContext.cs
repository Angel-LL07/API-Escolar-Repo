using API.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistencia
{
    public class APIContext :DbContext 
    {
        public APIContext(DbContextOptions<APIContext> context):base(context) {
        }

        DbSet<Estudiantes> Estudiantes { get; set; }
        DbSet<Calificaciones> Calificaciones { get; set; }
        DbSet<Materias> Materias { get; set; }
        DbSet<Carreras> Carreras { get; set; }
        DbSet<PeriodoEscolar> Periodos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
