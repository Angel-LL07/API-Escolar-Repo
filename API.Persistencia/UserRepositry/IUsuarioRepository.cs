using API.Aplicacion.DTOsUser;
using API.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistencia.UserRepositry
{
    public interface IUsuarioRepository
    {
        //IEnumerable<Usuario> GetUsuarios();
        //Usuario GetUsuario(int id);
        //bool EsUnico(string usuario);
        Task<Usuario> GetUsuarioAsync(Expression<Func<Usuario, bool>> match = null, string IncludeProperties = "");
        Task<IEnumerable<Usuario>> GetTodosAsync(Expression<Func<Usuario, bool>> match = null, Func<IQueryable<Usuario>, IOrderedQueryable<Usuario>> orderBy = null, string includeproperties = "");

        void EliminarAsync(Usuario usuario);
        Task<int> Save();
       
        Task<Usuario> Registro(UsuarioRegistroVM model);
        Task<UsuarioLoginRespuestaVM> login(UsuarioLoginVM model);
    }
}
