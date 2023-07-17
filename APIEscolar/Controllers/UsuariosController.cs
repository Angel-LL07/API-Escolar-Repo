using API.Aplicacion.DTOsUser;
using API.Dominio;
using API.Persistencia.Migrations;
using API.Persistencia.UserRepositry;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace APIEscolar.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioRepository _repository;
        private IMapper _mapper;
        private RespuestaAPI _respuestaAPI;
        public UsuariosController(IUsuarioRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
            this._respuestaAPI = new();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var usuarios = await _repository.GetTodosAsync();
            if (usuarios == null)
            {
                _respuestaAPI.StatusCode = HttpStatusCode.NotFound;
                _respuestaAPI.ErrorMessages.Add("No se encontraron usuarios");
                return BadRequest(_respuestaAPI);
            }
            var listaUsuarios = new List<UsuarioVM>();
            foreach (var usuario in usuarios)
            {
                listaUsuarios.Add(_mapper.Map<UsuarioVM>(usuario));
            }

            return Ok(listaUsuarios);
        }

        [AllowAnonymous]
        [HttpPost("RegistrarUsuario")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioRegistroVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existe = await _repository.GetUsuarioAsync(match: x => x.NombreUsuario == model.NombreUsuario);
            if (existe != null)
            {
                _respuestaAPI.ErrorMessages.Add("El usuario ya existe");
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                return BadRequest(_respuestaAPI);
            }

            var usuario = await _repository.Registro(model);
            if (usuario == null)
            {
                _respuestaAPI.ErrorMessages.Add("Error al al registrar usuario");
                _respuestaAPI.IsSuccess = false;
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_respuestaAPI);
            }

            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.IsSuccess = true;
            return Ok(_respuestaAPI);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(UsuarioLoginRespuestaVM))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> login([FromBody] UsuarioLoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var respuesta = await _repository.login(model);
            if (respuesta.Usuario == null)
            {
                _respuestaAPI.ErrorMessages.Add("Usuario o contraseña incorrecta");
                _respuestaAPI.StatusCode = HttpStatusCode.BadRequest;
                _respuestaAPI.IsSuccess = false;
                return BadRequest(_respuestaAPI);
            }

            _respuestaAPI.IsSuccess = true;
            _respuestaAPI.StatusCode = HttpStatusCode.OK;
            _respuestaAPI.Result = respuesta;
            return Ok(_respuestaAPI);

        }


        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{UsuarioId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult>Eliminar(int UsuarioId)
        {
            var existe = await _repository.GetUsuarioAsync(x => x.Id == UsuarioId);
            if(existe == null)
            {
                return BadRequest();
            }
            try
            {
                    _repository.EliminarAsync(existe);
              await _repository.Save();
            }
            catch
            {
                ModelState.AddModelError(" ", "Ocurrio un error al eliminar");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
