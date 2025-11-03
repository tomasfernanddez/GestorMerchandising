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
        public EfArchivoAdjuntoRepository(GestorMerchandisingContext context) : base(context)
        {
        }

        public IEnumerable<ArchivoAdjunto> ObtenerPorPedido(Guid idPedido)
        {
            return _dbSet
                .Where(a => a.IdPedido == idPedido)
                .OrderByDescending(a => a.FechaSubida)
                .ToList();
        }

        public IEnumerable<ArchivoAdjunto> ObtenerPorPedidoMuestra(Guid idPedidoMuestra)
        {
            return _dbSet
                .Where(a => a.IdPedidoMuestra == idPedidoMuestra)
                .OrderByDescending(a => a.FechaSubida)
                .ToList();
        }
    }
}