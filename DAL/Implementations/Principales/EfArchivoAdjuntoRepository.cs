using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DAL.Implementations.Base;
using DAL.Interfaces.Principales;
using DomainModel;

namespace DAL.Implementations.Principales
{
    public class EfArchivoAdjuntoRepository : EfRepository<ArchivoAdjunto>, IArchivoAdjuntoRepository
    {
        /// <summary>
        /// Inicializa el repositorio de archivos adjuntos con el contexto de datos proporcionado.
        /// </summary>
        /// <param name="context">Contexto de Entity Framework utilizado por la aplicación.</param>
        public EfArchivoAdjuntoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        /// <summary>
        /// Obtiene los archivos asociados a un pedido específico ordenados por fecha de subida.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Colección de archivos adjuntos del pedido.</returns>
        public IEnumerable<ArchivoAdjunto> ObtenerPorPedido(Guid idPedido)
        {
            return _dbSet
                .Where(a => a.IdPedido == idPedido)
                .OrderByDescending(a => a.FechaSubida)
                .ToList();
        }

        /// <summary>
        /// Obtiene los archivos asociados a un pedido de muestra ordenados por fecha de subida.
        /// </summary>
        /// <param name="idPedidoMuestra">Identificador del pedido de muestra.</param>
        /// <returns>Colección de archivos adjuntos del pedido de muestra.</returns>
        public IEnumerable<ArchivoAdjunto> ObtenerPorPedidoMuestra(Guid idPedidoMuestra)
        {
            return _dbSet
                .Where(a => a.IdPedidoMuestra == idPedidoMuestra)
                .OrderByDescending(a => a.FechaSubida)
                .ToList();
        }
    }
}