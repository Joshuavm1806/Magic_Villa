using MagicVilla_API.Datos;
using MagicVilla_API.Modelos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Obteniendo todas las Villas");
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) {
                _logger.LogError("Error con el id: " + id);
                return BadRequest();
            }

            //var villa = VillasStore.villaList.FirstOrDefault(villa => villa.id == id);
            var villa = _db.Villas.FirstOrDefault(villa => villa.id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa) ;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public ActionResult<VillaDTO> CrearVilla([FromBody] VillaDTO villaDTO) //Tipo de Datos (modelo), nombre que queramos
        {
            if (!ModelState.IsValid) //El modelo lo toma automaticamente del que está trabajando el action result
            {
                return BadRequest(ModelState);
            }

            if (_db.Villas.FirstOrDefault(villa => villa.Nombre.ToLower() == villaDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExistente", "El nombre de la villa ya ha sido utilizado");
                return BadRequest(ModelState);
            }

            if (villaDTO == null) {
                return BadRequest();
            }
            if (villaDTO.id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa model = new()
            {
                Nombre = villaDTO.Nombre,
                Ocupantes = villaDTO.Ocupantes,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Detalle = villaDTO.Detalle,
                Tarifa = villaDTO.Tarifa,
                Amenidad = villaDTO.Amenidad,
                ImagenUrl = villaDTO.ImagenUrl
            };

            _db.Villas.Add(model); //   Insert y
            _db.SaveChanges(); // cambios reflejados en la base datos

            return CreatedAtRoute("GetVilla", new {id = villaDTO.id}, villaDTO);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult DeleteVilla(int id)
        {
            var villa = _db.Villas.FirstOrDefault(villa => villa.id == id);

            if (id == 0)
            {
                return BadRequest();
            }

            if (villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent(); //Siempre se tiene que retornar un 204 en un delete
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            var villa = _db.Villas.FirstOrDefault(villa => villa.id == id);

            if (villaDTO == null || id != villaDTO.id)
            {
                return BadRequest();
            }

            if (villa == null)
            {
                return NotFound();
            }

            Villa model = new()
            {
                id = villaDTO.id,
                Nombre = villaDTO.Nombre,
                Ocupantes = villaDTO.Ocupantes,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Detalle = villaDTO.Detalle,
                Tarifa = villaDTO.Tarifa,
                Amenidad = villaDTO.Amenidad,
                ImagenUrl = villaDTO.ImagenUrl
            };

            _db.Villas.Update(model);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(villa => villa.id == id);

            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }

            VillaDTO villaDTO = new()
            {
                id = villa.id,
                Nombre = villa.Nombre,
                Ocupantes = villa.Ocupantes,
                MetrosCuadrados = villa.MetrosCuadrados,
                Detalle = villa.Detalle,
                Tarifa = villa.Tarifa,
                Amenidad = villa.Amenidad,
                ImagenUrl = villa.ImagenUrl
            };

            if (villa == null) return BadRequest();

            patchDTO.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            Villa modelo = new()
            {
                id = villaDTO.id,
                Nombre = villaDTO.Nombre,
                Ocupantes = villaDTO.Ocupantes,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Detalle = villaDTO.Detalle,
                Tarifa = villaDTO.Tarifa,
                Amenidad = villaDTO.Amenidad,
                ImagenUrl = villaDTO.ImagenUrl
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
