using System;
using System.Collections.Generic;
using DAL.Interfaces.Base;
using DomainModel;

namespace DAL.Interfaces.Principales
{
    /// <summary>
    /// Define operaciones de acceso a datos especializadas para archivos adjuntos.
    /// </summary>
    public interface IArchivoAdjuntoRepository : IRepository<ArchivoAdjunto>
    {
        /// <summary>
        /// Obtiene los archivos asociados a un pedido comercial específico.
        /// </summary>
        /// <param name="idPedido">Identificador del pedido.</param>
        /// <returns>Colección de archivos adjuntos.</returns>
        IEnumerable<ArchivoAdjunto> ObtenerPorPedido(Guid idPedido);

        /// <summary>
        /// Obtiene los archivos asociados a un pedido de muestra específico.
        /// </summary>
        /// <param name="idPedidoMuestra">Identificador del pedido de muestra.</param>
        /// <returns>Colección de archivos adjuntos.</returns>
        IEnumerable<ArchivoAdjunto> ObtenerPorPedidoMuestra(Guid idPedidoMuestra);
    }
}