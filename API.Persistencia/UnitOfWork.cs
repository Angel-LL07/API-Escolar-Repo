using API.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistencia
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly APIContext _dbcontext;
        public UnitOfWork(APIContext context)
        {
            _dbcontext = context;
        }
        private IGenericRepository<Calificaciones> _CalificacionesRepository;
        private IGenericRepository<Carreras> _CarrerasRepository;
        private IGenericRepository<Estudiantes> _EstudiantesRepository;
        private IGenericRepository<Materias> _MateriasRepository;
        private IGenericRepository<PeriodoEscolar> _PeriodoEscolarRepository;

        public IGenericRepository<Calificaciones> CalificacionesRepository
        {
            get { return _CalificacionesRepository ??=new GenericRepository<Calificaciones>(_dbcontext); }
        }
        public IGenericRepository<Carreras> CarrerasRepository
        {
            get
            {
               return _CarrerasRepository ??= new GenericRepository<Carreras>(_dbcontext);
            }
        }
        public IGenericRepository<Estudiantes> EstudiantesRepository
        {
            get
            {
                return _EstudiantesRepository ??= new GenericRepository<Estudiantes>(_dbcontext);
            }
        } 
        public IGenericRepository<Materias> MateriasRepository
        {
            get
            {
                return _MateriasRepository ??= new GenericRepository<Materias>(_dbcontext);
            }
        }

        public IGenericRepository<PeriodoEscolar> PeriodoEscolarRepository
        {
            get
            {
                return _PeriodoEscolarRepository ??= new GenericRepository<PeriodoEscolar>(_dbcontext);
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        private bool disposed = false;
        public void Dispose( bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbcontext.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
