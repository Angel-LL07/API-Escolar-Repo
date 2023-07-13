using API.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistencia
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Calificaciones> CalificacionesRepository { get; }
        IGenericRepository<Carreras> CarrerasRepository { get; }
        IGenericRepository<Estudiantes> EstudiantesRepository { get; }
        IGenericRepository<Materias> MateriasRepository { get; }
        IGenericRepository<PeriodoEscolar> PeriodoEscolarRepository { get; }
        Task<int> SaveAsync();
    }
}
