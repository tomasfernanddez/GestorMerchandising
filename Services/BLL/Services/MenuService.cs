using System;
using System.Collections.Generic;
using System.Linq;
using Services;

namespace Services.BLL.Services
{
    public class MenuItemConfig
    {
        public string Id { get; set; }
        public string Texto { get; set; }
        public List<string> FuncionesRequeridas { get; set; } = new List<string>();
        public bool SoloAdmin { get; set; }
        public List<MenuItemConfig> SubItems { get; set; } = new List<MenuItemConfig>();

        public MenuItemConfig CloneShallow()
        {
            return new MenuItemConfig
            {
                Id = Id,
                Texto = Texto,
                SoloAdmin = SoloAdmin,
                FuncionesRequeridas = new List<string>(FuncionesRequeridas)
            };
        }
    }

    public class MenuService
    {
        public IList<MenuItemConfig> ObtenerEstructuraMenu()
        {
            return new List<MenuItemConfig>
            {
                new MenuItemConfig
                {
                    Id = "menu.file",
                    Texto = "menu.file",
                    SubItems =
                    {
                        new MenuItemConfig
                        {
                            Id = "menu.file.logout",
                            Texto = "menu.file.logout"
                        },
                        new MenuItemConfig
                        {
                            Id = "menu.file.exit",
                            Texto = "menu.file.exit"
                        }
                    }
                },
                new MenuItemConfig
                {
                    Id = "menu.security",
                    Texto = "menu.security",
                    SubItems =
                    {
                        new MenuItemConfig
                        {
                            Id = "menu.security.profiles",
                            Texto = "menu.security.profiles",
                            FuncionesRequeridas = new List<string> { "SEG_PERFILES" }
                        },
                        new MenuItemConfig
                        {
                            Id = "menu.security.users",
                            Texto = "menu.security.users",
                            FuncionesRequeridas = new List<string> { "SEG_USUARIOS" }
                        },
                        new MenuItemConfig
                        {
                            Id = "menu.security.bitacora",
                            Texto = "menu.security.bitacora",
                            SoloAdmin = true
                        }
                    }
                },
                new MenuItemConfig
                {
                    Id = "menu.catalogs",
                    Texto = "menu.catalogs",
                    SubItems =
                    {
                        new MenuItemConfig
                        {
                            Id = "menu.catalogs.clients",
                            Texto = "menu.catalogs.clients",
                            FuncionesRequeridas = new List<string> { "CAT_CLIENTES" }
                        },
                        new MenuItemConfig
                        {
                            Id = "menu.catalogs.providers",
                            Texto = "menu.catalogs.providers",
                            FuncionesRequeridas = new List<string> { "CAT_PROVEEDORES" }
                        },
                        new MenuItemConfig
                        {
                            Id = "menu.catalogs.products",
                            Texto = "menu.catalogs.products",
                            FuncionesRequeridas = new List<string> { "CAT_PRODUCTOS" }
                        }
                    }
                },
                new MenuItemConfig
                {
                    Id = "menu.orders",
                    Texto = "menu.orders",
                    SubItems =
                    {
                        new MenuItemConfig
                        {
                            Id = "menu.orders.new",
                            Texto = "menu.orders.new",
                            FuncionesRequeridas = new List<string> { "PEDIDOS_VENTAS" }
                        },
                        new MenuItemConfig
                        {
                            Id = "menu.orders.list",
                            Texto = "menu.orders.list",
                            FuncionesRequeridas = new List<string> { "PEDIDOS_VENTAS" }
                        },
                        new MenuItemConfig
                        {
                            Id = "menu.orders.samples",
                            Texto = "menu.orders.samples",
                            FuncionesRequeridas = new List<string> { "PEDIDOS_MUESTRAS" }
                        }
                    }
                },
                new MenuItemConfig
                {
                    Id = "menu.reports",
                    Texto = "menu.reports",
                    FuncionesRequeridas = new List<string>
                    {
                        "REPORTES_OPERATIVOS",
                        "REPORTES_VENTAS",
                        "REPORTES_FINANCIEROS"
                    }
                },
                new MenuItemConfig
                {
                    Id = "menu.language",
                    Texto = "menu.language",
                    SubItems =
                    {
                        new MenuItemConfig
                        {
                            Id = "menu.language.es",
                            Texto = "menu.lang.es"
                        },
                        new MenuItemConfig
                        {
                            Id = "menu.language.en",
                            Texto = "menu.lang.en"
                        }
                    }
                }
            };
        }

        public IList<MenuItemConfig> FiltrarMenuPorPermisos(IEnumerable<MenuItemConfig> menuCompleto)
        {
            var resultado = new List<MenuItemConfig>();

            if (menuCompleto == null)
            {
                return resultado;
            }

            foreach (var item in menuCompleto)
            {
                var filtrado = ClonarFiltrando(item);
                if (filtrado != null)
                {
                    resultado.Add(filtrado);
                }
            }

            return resultado;
        }

        private MenuItemConfig ClonarFiltrando(MenuItemConfig item)
        {
            if (item == null)
            {
                return null;
            }

            if (!TieneAcceso(item))
            {
                return null;
            }

            var clon = item.CloneShallow();

            if (item.SubItems != null && item.SubItems.Count > 0)
            {
                foreach (var subItem in item.SubItems)
                {
                    var subFiltrado = ClonarFiltrando(subItem);
                    if (subFiltrado != null)
                    {
                        clon.SubItems.Add(subFiltrado);
                    }
                }

                if (item.SubItems.Count > 0 && clon.SubItems.Count == 0)
                {
                    return null;
                }
            }

            return clon;
        }

        private bool TieneAcceso(MenuItemConfig item)
        {
            if (item == null)
            {
                return false;
            }

            if (item.SoloAdmin && !SessionContext.EsAdministrador)
            {
                return false;
            }

            if (item.FuncionesRequeridas == null || item.FuncionesRequeridas.Count == 0)
            {
                return true;
            }

            return item.FuncionesRequeridas.Any(SessionContext.TieneFuncion);
        }
    }
}