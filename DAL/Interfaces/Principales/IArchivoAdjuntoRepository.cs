using System;
using System.Collections.Generic;
using DAL.Interfaces.Base;
using DomainModel;

namespace DAL.Interfaces.Principales
{
    public interface IArchivoAdjuntoRepository : IRepository<ArchivoAdjunto>
    {
        IEnumerable<ArchivoAdjunto> ObtenerPorPedido(Guid idPedido);
        IEnumerable<ArchivoAdjunto> ObtenerPorPedidoMuestra(Guid idPedidoMuestra);
    }
}