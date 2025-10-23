using DAL.Interfaces.Base;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces.Principales
{
    public interface IProductoRepository : IRepository<Producto>
    {
        // Métodos específicos para Producto
        IEnumerable<Producto> GetProductosPorCategoria(Guid idCategoria);
        Task<IEnumerable<Producto>> GetProductosPorCategoriaAsync(Guid idCategoria);
        IEnumerable<Producto> GetProductosPorProveedor(Guid idProveedor);
        Task<IEnumerable<Producto>> GetProductosPorProveedorAsync(Guid idProveedor);
        IEnumerable<Producto> BuscarPorNombre(string nombre);
        Task<IEnumerable<Producto>> BuscarPorNombreAsync(string nombre);
        bool ExisteNombre(string nombre);
        Task<bool> ExisteNombreAsync(string nombre);
        Producto ObtenerPorNombreExacto(string nombreNormalizado);
        Task<Producto> ObtenerPorNombreExactoAsync(string nombreNormalizado);
        IEnumerable<Producto> BuscarParaAutocomplete(string termino, int maxResultados = 10);
        Task<IEnumerable<Producto>> BuscarParaAutocompleteAsync(string termino, int maxResultados = 10);
        void RegistrarUso(Guid idProducto);
        Task RegistrarUsoAsync(Guid idProducto);
    }
}
