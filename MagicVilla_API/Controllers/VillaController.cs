using MagicVilla_API.Datos;
using MagicVilla_API.Modelos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Obteniendo todas las Villas");
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VillaDTO>>(villaList));
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0) {
                _logger.LogError("Error con el id: " + id);
                return BadRequest();
            }

            //var villa = VillasStore.villaList.FirstOrDefault(villa => villa.id == id);
            var villa = await _db.Villas.FirstOrDefaultAsync(villa => villa.id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDTO>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public async Task<ActionResult<VillaDTO>> CrearVilla([FromBody] VillaCreateDTO createDTO) //Tipo de Datos (modelo), nombre que queramos
        {
            if (!ModelState.IsValid) //El modelo lo toma automaticamente del que está trabajando el action result
            {
                return BadRequest(ModelState);
            }

            if (await _db.Villas.FirstOrDefaultAsync(villa => villa.Nombre.ToLower() == createDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExistente", "El nombre de la villa ya ha sido utilizado");
                return BadRequest(ModelState);
            }

            if (createDTO == null) {
                return BadRequest(createDTO);
            }

            Villa model = _mapper.Map<Villa>(createDTO);

            await _db.Villas.AddAsync(model); //   Insert y
            await _db.SaveChangesAsync(); // cambios reflejados en la base datos

            return CreatedAtRoute("GetVilla", new {id = model.id}, model);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteVilla(int id)
        {
            var villa = await _db.Villas.FirstOrDefaultAsync(villa => villa.id == id);

            if (id == 0)
            {
                return BadRequest();
            }

            if (villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent(); //Siempre se tiene que retornar un 204 en un delete
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || id != updateDTO.id)
            {
                return BadRequest();
            }

            Villa model = _mapper.Map<Villa>(updateDTO);

            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(villa => villa.id == id);

            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            if (villa == null) return BadRequest();

            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Villa modelo = _mapper.Map<Villa>(villaDTO);

            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
