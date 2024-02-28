using MagicVilla_API.Modelos;

namespace MagicVilla_API.Repositorios.IRepositorio
{
    public interface IVillaRepositorio : IRepositorio<Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
