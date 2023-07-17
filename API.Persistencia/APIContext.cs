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

        public DbSet<Estudiantes> Estudiantes { get; set; }
        public DbSet<Calificaciones> Calificaciones { get; set; }
        public DbSet<Materias> Materias { get; set; }
        public DbSet<Carreras> Carreras { get; set; }
        public DbSet<PeriodoEscolar> Periodos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
