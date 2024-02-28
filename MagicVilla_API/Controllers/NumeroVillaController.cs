using MagicVilla_API.Datos;
using MagicVilla_API.Modelos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_API.Repositorios.IRepositorio;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public NumeroVillaController(
            ILogger<NumeroVillaController> logger,
            IVillaRepositorio villaRepo,
            INumeroVillaRepositorio numeroRepo,
            IMapper mapper
        )
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumerosVillas()
        {
            try
            {
                _logger.LogInformation("Obteniendo números de las Villas");
                IEnumerable<NumeroVilla> numeroVillaList = await _numeroRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDTO>>(numeroVillaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("id:int", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error con el número de villa: " + id);
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //var villa = VillasStore.villaList.FirstOrDefault(villa => villa.id == id);
                var numeroVilla = await _numeroRepo.Obtener(numeroVilla => numeroVilla.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Resultado = _mapper.Map<NumeroVillaDTO>(numeroVilla);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return (_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDTO createDTO) //Tipo de Datos (modelo), nombre que queramos
        {
            try
            {
                if (!ModelState.IsValid) //El modelo lo toma automaticamente del que está trabajando el action result
                {
                    return BadRequest(ModelState);
                }

                if (await _numeroRepo.Obtener(numeroVilla => numeroVilla.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("NumeroDeVillaExistente", "El número de la villa ya ha sido utilizado");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(villa => villa.id == createDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El número de villa no existe");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                NumeroVilla model = _mapper.Map<NumeroVilla>(createDTO);

                model.FechaCreacion = DateTime.Now;
                model.FechaActualizacion = DateTime.Now;
                await _numeroRepo.Crear(model);
                _response.Resultado = model;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumeroVilla", new { id = model.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return (_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                var numeroVilla = await _numeroRepo.Obtener(numeroVilla => numeroVilla.VillaNo == id);

                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (numeroVilla == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _numeroRepo.Remover(numeroVilla);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response); //Siempre se tiene que retornar un 204 en un delete y update
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _villaRepo.Obtener(villa => villa.id == updateDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de villa no existe");
                    return BadRequest(ModelState);
                }

                NumeroVilla model = _mapper.Map<NumeroVilla>(updateDTO);

                await _numeroRepo.Actualizar(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }
    }
}
