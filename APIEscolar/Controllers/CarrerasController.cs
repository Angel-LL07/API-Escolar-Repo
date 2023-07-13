using API.Dominio;
using API.Persistencia;
using APIEscolar.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIEscolar.Controllers
{
    [Route("api/carreras")]
    [ApiController]
    public class CarrerasController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CarrerasController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerCarreras()
        {
            var lista = await _unitOfWork.CarrerasRepository.ObtenerTodosAsync();
            if (lista == null)
            {
                return NotFound();
            }
            var MuestraCarreras = new List<CarrerasVM>();
            foreach (var item in lista)
            {
                MuestraCarreras.Add(_mapper.Map<CarrerasVM>(item));
            }
            return Ok(MuestraCarreras);
        }

        [HttpGet("{CarreraId:int}", Name = "ObtenerCarreraById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObtenerCarreraById(int CarreraId)
        {
            var existe = await _unitOfWork.CarrerasRepository.ObtenerPorIdAsync(CarreraId);
            if (existe == null)
            {
                return NotFound();
            }
            _mapper.Map<CarrerasVM>(existe);
            return Ok(existe);
        }

        [HttpPost("AgregarCarrera")]
        [ProducesResponseType(201, Type = typeof(CarrerasVM))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AgregarCarrera([FromBody] CarrerasCrearVM model)
        {
            var existe = await _unitOfWork.CarrerasRepository.ObtenerAsync(match: x => x.NombreCarrera == model.NombreCarrera || x.NombreReducido == model.NombreReducido);
            if (existe != null)
            {
                ModelState.AddModelError(" ", $"La Carrera {model.NombreCarrera} ya existe");
                return BadRequest(ModelState);
            }
            var nuevacarrera = _mapper.Map<Carreras>(model);
            try
            {
                await _unitOfWork.CarrerasRepository.AgregarAsin(nuevacarrera);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(" ", $"Ocurrió un error al agregar la carrrera {model.NombreCarrera}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("ObtenerCarreraById", new { CarreraId = nuevacarrera.Id }, nuevacarrera);

        }

        [HttpPatch("{CarreraId:int}", Name = "ActualizarCarrera")]
        [ProducesResponseType(200, Type = typeof(CarrerasVM))]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ActualizarCarrera(int CarreraId, [FromBody] CarrerasVM model)
        {
            var existe = await _unitOfWork.CarrerasRepository.ObtenerAsync(match: x => x.Id == CarreraId);
            if (existe is null)
            {
                ModelState.AddModelError("", "Usuario no encontrado");
                return StatusCode(404, ModelState);
            }
            var modificado = _mapper.Map<Carreras>(model);
            try
            {
                await _unitOfWork.CarrerasRepository.ActualizarAsync(modificado, CarreraId);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return NoContent();
        }
        [HttpDelete("{CarreraId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarCarrera(int CarreraId)
        {
            var existe = await _unitOfWork.CarrerasRepository.ObtenerAsync(match: x => x.Id == CarreraId);
            if (existe == null)
            {
                return BadRequest();
            }
            try
            {
                _unitOfWork.CarrerasRepository.EliminarAsyn(existe);
                _unitOfWork.SaveAsync();
            }
            catch
            {
                ModelState.AddModelError(" ", $"Ocurrio un error al borrar la carrera {existe.NombreCarrera}");
                return BadRequest();
            }
            return NoContent();
        }
    }
}
