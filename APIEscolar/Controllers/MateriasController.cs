using API.Dominio;
using API.Persistencia;
using APIEscolar.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIEscolar.Controllers
{
    [Route("api/materias")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        public MateriasController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerMaterias()
        {
            var Lista = await _unitOfWork.MateriasRepository.ObtenerTodosAsync();
            var Materias = new List<MateriasVM>();

            foreach (var newlist in Lista)
            {
                Materias.Add(_mapper.Map<MateriasVM>(newlist));
            }
            return Ok(Materias);
        }

        [HttpGet("{MateriaId:int}", Name = "ObtenerId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerPorId(int MateriaId)
        {
            var Existe = await _unitOfWork.MateriasRepository.ObtenerPorIdAsync(MateriaId);
            if (Existe == null)
            {
                return NotFound();
            }
            var muestra = _mapper.Map<MateriasVM>(Existe);
            return Ok(muestra);

        }

        [HttpPost("AgregarMateria")]
        [ProducesResponseType(201, Type = typeof(MateriasVM))]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AgregarMateria([FromBody] MateriasCrearVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(" ", "Todos los campos son necesarios");
                return BadRequest(ModelState);
            }
            var Materia = await _unitOfWork.MateriasRepository.ObtenerAsync(match: x => x.NombreMateria == model.NombreMateria || x.ClaveMateria == model.ClaveMateria);
            if (Materia != null)
            {
                ModelState.AddModelError(" ", $"La materia {model.NombreMateria} ya existe");
                return BadRequest(ModelState);
            }
            var registro = _mapper.Map<Materias>(model);
            try
            {

                await _unitOfWork.MateriasRepository.AgregarAsin(registro);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(" ", "Hubo un error");
                return BadRequest(ModelState);
            }


            return CreatedAtRoute("ObtenerId", new { MateriaId = registro.Id }, registro);

        }

        [HttpPatch("{MateriaId:int}", Name = "ActualizarMateria")]
        [ProducesResponseType(200, Type = typeof(MateriasVM))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ActualizarMateria(int MateriaId, [FromBody] MateriasVM model)
        {
            var materia = await _unitOfWork.MateriasRepository.ObtenerPorIdAsync(MateriaId);
            if (materia == null)
            {
                ModelState.AddModelError(" ", "Materia no encontrado");
                return BadRequest(ModelState);
            }
            var actualizado = _mapper.Map<Materias>(materia);
            try
            {
                await _unitOfWork.MateriasRepository.ActualizarAsync(actualizado, actualizado.Id);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                ModelState.AddModelError(" ", "Ocurrio un error");
                return BadRequest(ModelState);
            }
            return NoContent();
        }
        [HttpDelete("{MateriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EliminarMateria(int MateriaId)
        {
            var materia = await _unitOfWork.MateriasRepository.ObtenerAsync(match: x => x.Id == MateriaId);
            if (materia == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.MateriasRepository.EliminarAsyn(materia);
                _unitOfWork.SaveAsync();
            }
            catch
            {
                ModelState.AddModelError(" ", $"Ocurrio un error al borrar la materia {materia.NombreMateria}");
                return StatusCode(404, ModelState);
            }

            return NoContent();

        }
    }
}
