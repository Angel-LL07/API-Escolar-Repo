using API.Dominio;
using API.Persistencia;
using APIEscolar.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIEscolar.Controllers
{
    [Route("api/calificaciones")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CalificacionesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{EstudianteNoControl:int}", Name = "CalificacionesByNoControl")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CalificacionesByNoControl(int EstudianteNoControl)
        {
            var lista = await _unitOfWork.CalificacionesRepository.ObtenerTodosAsync(match: x => x.EstudiantesNoControl == EstudianteNoControl);
            if (lista == null)
            {
                return NotFound();
            }

            var muestra = new List<CalificacionesVM>();
            foreach (var item in lista) 
            {
                muestra.Add(_mapper.Map<CalificacionesVM>(item));
            }

            return Ok(lista);
        }

        [HttpGet("{EstudianteNoControl:int},{PeriodoId:int}", Name = "CalificacionesByPeriodo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CalificacionesByPeriodo(int EstudianteNoControl,int PeriodoId)
        {
            var existe = await _unitOfWork.CalificacionesRepository.ObtenerAsync(match: x => x.EstudiantesNoControl == EstudianteNoControl &&x.PeriodoId == PeriodoId);
            if (existe == null)
            {
                return NotFound();
            }
            _mapper.Map<CalificacionesVM>(existe);
            return Ok(existe);
        }

        [HttpPost("AgregarCalificación")]
        [ProducesResponseType(201, Type = typeof(CalificacionesVM))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AgregarCalificacion([FromBody] CalificacionesCreacionVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(" ", "Todos los campos son obligatorios");
                return BadRequest(ModelState);
            }
            var existe = await _unitOfWork.CalificacionesRepository.ObtenerAsync(match: x =>x.MateriaId==model.MateriaId && x.PeriodoId==model.PeriodoId &&x.EstudiantesNoControl==model.EstudiantesNoControl);
            if (existe != null)
            {
                ModelState.AddModelError(" ", $"El calificacion para la materia {model.MateriaId} ya ha sido registrada para el periodo deseado,puede actualizar la calificación.");
                return BadRequest(ModelState);
            }
            var materia = await _unitOfWork.MateriasRepository.ObtenerAsync(match: x => x.Id == model.MateriaId);
            if (materia == null)
            {
                ModelState.AddModelError(" ", $"La materia ingresada no existe");
                return BadRequest(ModelState);
            }
            var NuevaCalificacion = _mapper.Map<Calificaciones>(model);
            try
            {
                await _unitOfWork.CalificacionesRepository.AgregarAsin(NuevaCalificacion);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(" ", $"Ocurrió un error al agregar la calificacion al estudiante {model.EstudiantesNoControl}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("CalificacionesByPeriodo", new { EstudianteNoControl = NuevaCalificacion.EstudiantesNoControl, PeriodoId = NuevaCalificacion.PeriodoId }, NuevaCalificacion);

        }

        [HttpDelete("{NoControl:int},{PeriodoId:int},{MateriaId}",Name = "EliminarCalificacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarCalificacion(int NoControl,int PeriodoId,int MateriaId)
        {
            var existe = await _unitOfWork.CalificacionesRepository
                                    .ObtenerAsync(match: x => x.EstudiantesNoControl == NoControl 
                                                  && x.PeriodoId == PeriodoId && x.MateriaId==MateriaId);
            if (existe == null)
            {
                ModelState.AddModelError(" ", "Registro de calificacion no encontrado");
                return BadRequest(ModelState);
            }
            try
            {
                _unitOfWork.CalificacionesRepository.EliminarAsyn(existe);
                _unitOfWork.SaveAsync();
            }
            catch
            {
                ModelState.AddModelError(" ", $"Ocurrio un error al borrar la calificacion");
                return BadRequest();
            }
            return NoContent();
        }



        [HttpPatch("{NoControl:int},{PeriodoId:int},{MateriaId:int}", Name = "ActualizarCalificaciones")]
        [ProducesResponseType(200, Type = typeof(CalificacionesActualizaVM))]
        [ProducesResponseType(StatusCodes.Status304NotModified)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ActualizarCalificaciones(int NoControl, int PeriodoId,int MateriaId, [FromBody] CalificacionesActualizaVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existe = await _unitOfWork.CalificacionesRepository.ObtenerAsync(match: x => x.EstudiantesNoControl == NoControl && x.MateriaId == MateriaId && x.PeriodoId == PeriodoId);
            if (existe is null)
            {
                ModelState.AddModelError("", "Registro de calificacion no encontrada");
                return StatusCode(404, ModelState);
            }
            var materia = await _unitOfWork.MateriasRepository.ObtenerAsync(match: x => x.Id == model.MateriaId);
            if (materia == null)
            {
                ModelState.AddModelError(" ", $"La materia ingresada no existe");
                return BadRequest(ModelState);
            }
            var modificado = _mapper.Map<Calificaciones>(model);
            modificado.id = existe.id;
            try
            {
                await _unitOfWork.CalificacionesRepository.ActualizarAsync(modificado,modificado.id);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return NoContent();
        }


















        [HttpDelete("{NoControl:int},{PeriodoId:int}", Name = "EliminarCalificacionPeriodo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarCalificacionPeriodo(int NoControl, int PeriodoId)
        {
            var existe = await _unitOfWork.CalificacionesRepository
                                    .ObtenerAsync(match: x => x.EstudiantesNoControl == NoControl
                                                  && x.PeriodoId == PeriodoId );
            if (existe == null)
            {
                ModelState.AddModelError(" ", "Registro de calificacion no encontrado");
                return BadRequest(ModelState);
            }
            try
            {
                _unitOfWork.CalificacionesRepository.EliminarAsyn(existe);
                _unitOfWork.SaveAsync();
            }
            catch
            {
                ModelState.AddModelError(" ", $"Ocurrio un error al borrar la calificacion");
                return BadRequest();
            }
            return NoContent();
        }

    }
}
