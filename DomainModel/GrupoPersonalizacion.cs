using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DomainModel
{
    /// <summary>
    /// Patrón Composite - Composite (Contenedor)
    /// Representa un grupo/paquete de personalizaciones que pueden tratarse como una unidad
    /// Ejemplo: "Paquete Corporativo" = Logo frontal + Logo trasero + Logo manga
    /// </summary>
    [Table("GrupoPersonalizacion")]
    public class GrupoPersonalizacion : IComponentePersonalizacion
    {
        [Key]
        public Guid IdGrupoPersonalizacion { get; set; } = Guid.NewGuid();

        /// <summary>
        /// ID del detalle de pedido al que pertenece este grupo
        /// </summary>
        public Guid IdDetallePedido { get; set; }

        /// <summary>
        /// Nombre descriptivo del grupo (ej: "Paquete Premium", "Personalización Completa")
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción del grupo de personalizaciones
        /// </summary>
        [StringLength(500)]
        public string Descripcion { get; set; }

        /// <summary>
        /// Costo adicional por el grupo (además de la suma de los componentes)
        /// Puede ser 0 si solo se suma el costo de los componentes individuales
        /// O puede ser negativo para aplicar un descuento por paquete
        /// </summary>
        public decimal CostoAdicionalGrupo { get; set; } = 0;

        /// <summary>
        /// Indica si este es un grupo activo
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// ID del grupo padre (si este grupo está contenido en otro grupo)
        /// Permite jerarquías multinivel
        /// </summary>
        public Guid? IdGrupoPadre { get; set; }

        // Navegación
        public virtual PedidoDetalle DetallePedido { get; set; }
        public virtual GrupoPersonalizacion GrupoPadre { get; set; }
        public virtual ICollection<GrupoPersonalizacion> GruposHijos { get; set; } = new List<GrupoPersonalizacion>();

        /// <summary>
        /// Relación con logos individuales que pertenecen a este grupo
        /// Se maneja mediante una tabla de relación many-to-many
        /// </summary>
        public virtual ICollection<GrupoPersonalizacionLogo> LogosAsociados { get; set; } = new List<GrupoPersonalizacionLogo>();

        #region Implementación de IComponentePersonalizacion (Patrón Composite)

        /// <summary>
        /// ID del componente (mapea a IdGrupoPersonalizacion)
        /// </summary>
        [NotMapped]
        public Guid Id => IdGrupoPersonalizacion;

        /// <summary>
        /// Un grupo de personalizaciones es un contenedor (composite)
        /// </summary>
        [NotMapped]
        public bool EsGrupo => true;

        /// <summary>
        /// Obtiene la descripción del grupo
        /// </summary>
        public string ObtenerDescripcion()
        {
            if (!string.IsNullOrWhiteSpace(Descripcion))
                return $"{Nombre}: {Descripcion}";

            return Nombre;
        }

        /// <summary>
        /// Calcula el costo total del grupo de manera recursiva
        /// Suma el costo de todos los componentes + el costo adicional del grupo
        /// </summary>
        public decimal CalcularCosto()
        {
            decimal costoTotal = CostoAdicionalGrupo;

            // Sumar el costo de todos los componentes hijos
            foreach (var componente in ObtenerComponentes())
            {
                costoTotal += componente.CalcularCosto();
            }

            return costoTotal;
        }

        /// <summary>
        /// Obtiene todos los componentes de este grupo (logos y subgrupos)
        /// </summary>
        public IEnumerable<IComponentePersonalizacion> ObtenerComponentes()
        {
            var componentes = new List<IComponentePersonalizacion>();

            // Agregar logos individuales
            if (LogosAsociados != null)
            {
                foreach (var logoAsociado in LogosAsociados.Where(la => la.Logo != null))
                {
                    componentes.Add(logoAsociado.Logo);
                }
            }

            // Agregar subgrupos
            if (GruposHijos != null)
            {
                componentes.AddRange(GruposHijos.Where(g => g.Activo));
            }

            return componentes;
        }

        /// <summary>
        /// Agrega un componente al grupo
        /// </summary>
        public void Agregar(IComponentePersonalizacion componente)
        {
            if (componente == null)
                throw new ArgumentNullException(nameof(componente));

            if (componente.Id == this.Id)
                throw new InvalidOperationException("Un grupo no puede agregarse a sí mismo");

            if (componente is LogosPedido logo)
            {
                // Agregar logo individual
                if (!LogosAsociados.Any(la => la.IdLogo == logo.IdLogoPedido))
                {
                    LogosAsociados.Add(new GrupoPersonalizacionLogo
                    {
                        IdGrupo = this.IdGrupoPersonalizacion,
                        IdLogo = logo.IdLogoPedido,
                        Logo = logo
                    });
                }
            }
            else if (componente is GrupoPersonalizacion subgrupo)
            {
                // Agregar subgrupo
                if (!GruposHijos.Any(g => g.IdGrupoPersonalizacion == subgrupo.IdGrupoPersonalizacion))
                {
                    subgrupo.IdGrupoPadre = this.IdGrupoPersonalizacion;
                    GruposHijos.Add(subgrupo);
                }
            }
            else
            {
                throw new ArgumentException("Tipo de componente no soportado", nameof(componente));
            }
        }

        /// <summary>
        /// Elimina un componente del grupo
        /// </summary>
        public void Eliminar(IComponentePersonalizacion componente)
        {
            if (componente == null)
                throw new ArgumentNullException(nameof(componente));

            if (componente is LogosPedido logo)
            {
                // Eliminar logo individual
                var logoAsociado = LogosAsociados.FirstOrDefault(la => la.IdLogo == logo.IdLogoPedido);
                if (logoAsociado != null)
                {
                    LogosAsociados.Remove(logoAsociado);
                }
            }
            else if (componente is GrupoPersonalizacion subgrupo)
            {
                // Eliminar subgrupo
                var subgrupoExistente = GruposHijos.FirstOrDefault(g => g.IdGrupoPersonalizacion == subgrupo.IdGrupoPersonalizacion);
                if (subgrupoExistente != null)
                {
                    subgrupoExistente.IdGrupoPadre = null;
                    GruposHijos.Remove(subgrupoExistente);
                }
            }
        }

        #endregion

        #region Métodos de utilidad

        /// <summary>
        /// Obtiene todos los logos del grupo de manera recursiva (incluyendo subgrupos)
        /// </summary>
        public IEnumerable<LogosPedido> ObtenerTodosLosLogos()
        {
            var logos = new List<LogosPedido>();

            // Logos directos
            if (LogosAsociados != null)
            {
                logos.AddRange(LogosAsociados.Where(la => la.Logo != null).Select(la => la.Logo));
            }

            // Logos de subgrupos (recursivo)
            if (GruposHijos != null)
            {
                foreach (var subgrupo in GruposHijos.Where(g => g.Activo))
                {
                    logos.AddRange(subgrupo.ObtenerTodosLosLogos());
                }
            }

            return logos;
        }

        /// <summary>
        /// Cuenta el total de logos en este grupo y subgrupos
        /// </summary>
        public int ContarLogos()
        {
            return ObtenerTodosLosLogos().Count();
        }

        #endregion
    }

    /// <summary>
    /// Tabla de relación many-to-many entre GrupoPersonalizacion y LogosPedido
    /// Permite que un logo pertenezca a múltiples grupos
    /// </summary>
    [Table("GrupoPersonalizacionLogo")]
    public class GrupoPersonalizacionLogo
    {
        [Key]
        [Column(Order = 0)]
        public Guid IdGrupo { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid IdLogo { get; set; }

        /// <summary>
        /// Orden del logo dentro del grupo (para mostrar en orden específico)
        /// </summary>
        public int Orden { get; set; } = 0;

        /// <summary>
        /// Fecha en que se agregó el logo al grupo
        /// </summary>
        public DateTime FechaAgregado { get; set; } = DateTime.Now;

        // Navegación
        public virtual GrupoPersonalizacion Grupo { get; set; }
        public virtual LogosPedido Logo { get; set; }
    }
}
