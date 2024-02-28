using MagicVilla_API.Modelos.DTO;

namespace MagicVilla_API.Datos
{
    public static class VillasStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO{id=1, Nombre="Vista a la piscina", Ocupantes=3},
            new VillaDTO{id=2, Nombre="Vista a la playa", Ocupantes=4},
            new VillaDTO{id=3, Nombre="Vista al mar", Ocupantes=5}
        };
    }
}
