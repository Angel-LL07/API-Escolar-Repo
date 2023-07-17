using API.Dominio;
using API.Persistencia;
using APIEscolar.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIEscolar.Controllers
{
    [Route("api/estudiantes")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public EstudiantesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [ResponseCache(CacheProfileName ="20Seg")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerEstudiantes()
        {
            var lista = await _unitOfWork.EstudiantesRepository.ObtenerTodosAsync();
            if (lista == null)
            {
                return NotFound();
            }
            var MuestraEstudiantes = new List<EstudiantesVM>();
            foreach (var item in lista)
            {
                MuestraEstudiantes.Add(_mapper.Map<EstudiantesVM>(item));
            }
            return Ok(MuestraEstudiantes);
        }

        [ResponseCache(CacheProfileName = "20Seg")]
        [AllowAnonymous]
        [HttpGet("{EstudianteNoControl:int}", Name = "ObtenerEstudianteById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerEstudianteById(int EstudianteNoControl)
        {
            var existe = await _unitOfWork.EstudiantesRepository.ObtenerPorIdAsync(EstudianteNoControl);
            if (existe == null)
            {
                return NotFound();
            }
          var estudiante=  _mapper.Map<EstudiantesVM>(existe);
            return Ok(estudiante);
        }

        [Authorize(Roles ="ADMIN")]
        [HttpPost("AgregarEstudiante")]
        [ProducesResponseType(201, Type = typeof(EstudiantesVM))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AgregarEstudiante([FromBody] EstudiantesCreacionVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(" ", "Todos los campos son obligatorios");
                return BadRequest(ModelState);
            }
            if (model.Sexo.Length > 1)
            {
                ModelState.AddModelError(" ", "En el campo sexo Ingrese 'M' (Masculino) o 'F' (Femenino)");
                return BadRequest(ModelState);
            }
            var existe = await _unitOfWork.EstudiantesRepository.ObtenerAsync(match: x => x.Curp==model.Curp);
            if (existe != null)
            {
                ModelState.AddModelError(" ", $"El estudiante con curp {model.Curp} ya existe");
                return BadRequest(ModelState);
            }
            var carreras = await _unitOfWork.CarrerasRepository.ObtenerAsync(match :x=>x.Id==model.CarreraId);
            if (carreras ==null)
            {
                ModelState.AddModelError(" ", $"La carrera ingresada no existe");
                return BadRequest(ModelState);
            }
            var NuevoEstudiante = _mapper.Map<Estudiantes>(model);
            try
            {
                await _unitOfWork.EstudiantesRepository.AgregarAsin(NuevoEstudiante);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(" ", $"Ocurrió un error al agregar el estudiantec {model.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("ObtenerEstudianteById", new { EstudianteNoControl = NuevoEstudiante.NoControl }, NuevoEstudiante);

        }

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("{NoControl:int}", Name = "ActualizarEstudiante")]
        [ProducesResponseType(200, Type = typeof(EstudiantesVM))]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ActualizarEstudiante(int NoControl, [FromBody] EstudiantesVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existe = await _unitOfWork.EstudiantesRepository.ObtenerAsync(match: x => x.NoControl == NoControl);
            if (existe is null)
            {
                ModelState.AddModelError("", "Estudiante no encontrado");
                return StatusCode(404, ModelState);
            }
            if (model.Sexo.Length > 1)
            {
                ModelState.AddModelError(" ", "En el campo sexo Ingrese 'M' (Masculino) o 'F' (Femenino)");
                return BadRequest(ModelState);
            }
            var ExisteCarrera = await _unitOfWork.CarrerasRepository.ObtenerAsync(match: x => x.Id == model.CarreraId);
            if (ExisteCarrera == null)
            {
                ModelState.AddModelError(" ", $"La carrera ingresada no existe");
                return BadRequest(ModelState);
            }
            var EstuidianteEdit = _mapper.Map<Estudiantes>(model);
            try
            {
                await _unitOfWork.EstudiantesRepository.ActualizarAsync(EstuidianteEdit, NoControl);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return NoContent();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{NoControl:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarEstudiante(int NoControl)
        {
            var existe = await _unitOfWork.EstudiantesRepository.ObtenerAsync(match: x => x.NoControl == NoControl);
            if (existe == null)
            {
                return BadRequest();
            }
            try
            {
                _unitOfWork.EstudiantesRepository.EliminarAsyn(existe);
                _unitOfWork.SaveAsync();
            }
            catch
            {
                ModelState.AddModelError(" ", $"Ocurrio un error al borrar al estudiante {existe.Nombre}");
                return BadRequest();
            }
            return NoContent();
        }
    }
}
