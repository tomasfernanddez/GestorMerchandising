using System;

namespace BLL.Helpers
{
    /// <summary>
    /// Inicializa una nueva instancia de EstadoCalculado.
    /// </summary>
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