using API.Aplicacion.DTOsUser;
using API.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using APIEscolar;
using XSystem.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace API.Persistencia.UserRepositry
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private APIContext _context;
        private string _keey;
        public UsuarioRepository(APIContext context ,IConfiguration configuration)
        {
            _context = context;
            _keey = configuration.GetSection("Keyy:Oculta").ToString();
        }


        public async Task<Usuario> GetUsuarioAsync(Expression<Func<Usuario, bool>> match = null, string IncludeProperties = "")
        {
            IQueryable<Usuario> query = _context.Usuarios;
            if (match != null)
            {
                query = query.Where(match);
            }
            foreach (var item in IncludeProperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(item);
            }
            return await query.FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Usuario>> GetTodosAsync(Expression<Func<Usuario,bool>> match =null,Func<IQueryable<Usuario>,IOrderedQueryable<Usuario>> orderBy=null,string includeproperties="")
        {
            IQueryable<Usuario> query = _context.Usuarios;
            if(match != null)
            {
                query = query.Where(match);
            }
            foreach (var item in includeproperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
            {
                    query=query.Include(item);
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        
        }

        public async Task<UsuarioLoginRespuestaVM> login(UsuarioLoginVM model)
        {
            var password = ObtenerMD5(model.Contraseña);
            
            var usuario = _context.Usuarios.FirstOrDefault(x=>x.NombreUsuario==model.NombreUsuario && x.Contraseña==password);
            if(usuario == null)
            {
                return new UsuarioLoginRespuestaVM()
                {
                    Usuario = null,
                    Token = ""
                };
            }

            var maejadortoken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_keey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,usuario.NombreUsuario),
                    new Claim(ClaimTypes.Role,usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new ( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = maejadortoken.CreateToken(tokenDescriptor);
            var usuariorespuesta = new UsuarioLoginRespuestaVM()
            {
                Token = maejadortoken.WriteToken(token),
                Usuario = usuario
            };
            return usuariorespuesta;
        }

        public async Task<Usuario> Registro(UsuarioRegistroVM model)
        {
            var password = ObtenerMD5(model.Contraseña);
            Usuario usuario = new Usuario()
            {
                NombreUsuario = model.NombreUsuario,
                Nombre = model.NombreUsuario,
                Contraseña = password,
                Role = model.Role
            };
             await _context.AddAsync(usuario);
             _context.SaveChanges();
            usuario.Contraseña = password;

            return usuario;
        }

        public async  void EliminarAsync(Usuario model)
        {
           _context.Usuarios.Remove(model);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
        public static string ObtenerMD5(string valor)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();
            return resp;

        }


    }
}
