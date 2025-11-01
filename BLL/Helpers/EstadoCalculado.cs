using System;

namespace BLL.Helpers
{
    public sealed class EstadoCalculado
    {
        public EstadoCalculado(Guid? idEstado, string nombreEstado)
        {
            IdEstado = idEstado;
            NombreEstado = nombreEstado;
        }

        public Guid? IdEstado { get; }

        public string NombreEstado { get; }
    }
}