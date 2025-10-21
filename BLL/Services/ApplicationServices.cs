using BLL.Factories;
using BLL.Interfaces;
using DAL;
using DAL.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace BLL.Services
{
    public static class ApplicationServices
    {
        // =====================================================================
        // TESTS DE CONEXIÓN Y DIAGNÓSTICO
        // =====================================================================

        /// <summary>
        /// Test básico: intenta crear un contexto.
        /// </summary>
        public static (bool exito, string mensaje) TestConexionBasico()
        {
            try
            {
                using (var ctx = new GestorMerchandisingContext(GetConnectionString()))
                {
                    return (true, "Contexto creado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error creando contexto: {ex.Message}");
            }
        }

        /// <summary>
        /// Test de conexión a base de datos: verifica que la BD existe.
        /// </summary>
        public static (bool exito, string mensaje) TestConexionBD()
        {
            try
            {
                using (var ctx = new GestorMerchandisingContext(GetConnectionString()))
                {
                    bool existe = ctx.Database.Exists();
                    return existe
                        ? (true, "Conexión exitosa - Base de datos encontrada")
                        : (false, "La base de datos no existe o no es accesible");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

        // =====================================================================
        // INICIALIZACIÓN DEL SISTEMA
        // =====================================================================

        /// <summary>
        /// Inicializa el sistema con datos básicos (perfiles y usuario admin).
        /// </summary>
        public static (bool exito, string mensaje) InicializarSistema()
        {
            try
            {
                EnsureDatabaseUpToDate();
                using (var ctx = new GestorMerchandisingContext(GetConnectionString()))
                {
                    if (!ctx.Database.Exists())
                    {
                        return (false, "La base de datos no existe. Créela primero.");
                    }

                    // Usar el método del contexto que ya inicializa perfiles + admin
                    ctx.InicializarDatos();

                    return (true, "Sistema inicializado correctamente con datos básicos");
                }
            }
            catch (Exception ex) { return (false, "Error al inicializar: " + FullError(ex)); }
        }

        // =====================================================================
        // VERIFICACIÓN DE AUTENTICACIÓN
        // =====================================================================

        /// <summary>
        /// Verifica que el sistema de autenticación esté correctamente configurado.
        /// </summary>
        public static (bool esValido, string mensaje) VerificarSistemaAutenticacion()
        {
            try
            {
                EnsureDatabaseUpToDate();
                using (var ctx = new GestorMerchandisingContext(GetConnectionString()))
                using (var uow = new EfUnitOfWork(ctx))
                {
                    // Verificar perfiles
                    var perfiles = uow.Perfiles.GetPerfilesActivos();
                    if (!perfiles.Any())
                    {
                        return (false, "No existen perfiles en el sistema. Ejecute InicializarSistema() primero.");
                    }

                    // Verificar usuarios
                    var usuarios = uow.Usuarios.GetUsuariosActivos();
                    if (!usuarios.Any())
                    {
                        return (false, "No existen usuarios activos. Ejecute InicializarSistema() primero.");
                    }

                    // Verificar usuario admin
                    var admin = uow.Usuarios.GetUsuarioPorNombre("admin");
                    if (admin == null)
                    {
                        return (false, "Usuario administrador no encontrado");
                    }

                    return (true, $"Sistema configurado correctamente: {perfiles.Count()} perfiles, {usuarios.Count()} usuarios activos");
                }
            }
            catch (Exception ex) { return (false, "Error verificando autenticación: " + FullError(ex)); }
        }

        /// <summary>
        /// Prueba de login con el usuario administrador por defecto.
        /// </summary>
        public static (bool exito, string mensaje) TestLoginAdmin()
        {
            IAutenticacionService auth = null;
            try
            {
                EnsureDatabaseUpToDate();
                auth = ServiceFactory.CrearAutenticacionService();

                var r = auth.Login("admin", "admin", "127.0.0.1");
                if (!r.EsValido) return (false, r.Mensaje);

                auth.Logout(r.Usuario.IdUsuario, "127.0.0.1");
                return (true, $"Login exitoso: {r.Usuario.NombreCompleto}");
            }
            catch (Exception ex) 
            {
                return (false, "Error en test de login: " + FullError(ex)); 
            }
            finally
            {
                (auth as IDisposable)?.Dispose(); // si la implementación concreta es IDisposable, se libera
            }
        }

        // =====================================================================
        // VERIFICACIÓN DETALLADA
        // =====================================================================

        /// <summary>
        /// Verificación exhaustiva de toda la configuración del sistema.
        /// </summary>
        public static (bool esValido, string mensaje) VerificarConfiguracionDetallada()
        {
            try
            {
                // Test 1: Conexión
                using (var ctx = new GestorMerchandisingContext(GetConnectionString()))
                {
                    if (!ctx.Database.Exists())
                        return (false, "La base de datos no existe");

                    // Test 2: UnitOfWork
                    using (var unitOfWork = new EfUnitOfWork(ctx))
                    {
                        if (unitOfWork == null)
                            return (false, "No se puede crear UnitOfWork");

                        // Test 3: Servicios
                        var clienteService = new ClienteService(unitOfWork);
                        if (clienteService == null)
                            return (false, "No se puede crear ClienteService");

                        // Test 4: Operaciones básicas
                        int countClientes = clienteService.ObtenerClientesActivos().Count();
                        int countPerfiles = unitOfWork.Perfiles.GetPerfilesActivos().Count();
                        int countUsuarios = unitOfWork.Usuarios.GetUsuariosActivos().Count();

                        return (true, $"Sistema completamente funcional: {countClientes} clientes, {countPerfiles} perfiles, {countUsuarios} usuarios");
                    }
                }
            }
            catch (Exception ex) { return (false, "Error durante la verificación: " + FullError(ex)); }
        }

        // =====================================================================
        // TEST COMPLETO DE ARQUITECTURA
        // =====================================================================

        /// <summary>
        /// Ejecuta un test completo de toda la arquitectura base del sistema.
        /// </summary>
        public static (bool exito, string mensaje) TestArquitecturaBase()
        {
            try
            {

                // Test 1: Inicialización
                var (exitoInit, mensajeInit) = InicializarSistema();
                if (!exitoInit)
                    return (false, $"Falló inicialización: {mensajeInit}");

                // Test 2: Verificación del sistema de autenticación
                var (exitoAuth, mensajeAuth) = VerificarSistemaAutenticacion();
                if (!exitoAuth)
                    return (false, $"Falló verificación de autenticación: {mensajeAuth}");

                // Test 3: Test de login
                var (exitoLogin,  mensajeLogin) = TestLoginAdmin();
                if (!exitoLogin)
                    return (false, $"Falló test de login: {mensajeLogin}");

                return (true, "✓ Arquitectura base funcionando correctamente - Todos los tests pasaron");
            }
            catch (Exception ex) { return (false, "Error en test de arquitectura: " + FullError(ex)); }
        }

        // =====================================================================
        // INFORMACIÓN DEL SISTEMA
        // =====================================================================

        /// <summary>
        /// Obtiene información sobre la configuración actual del sistema.
        /// </summary>
        public static string ObtenerInformacionConfiguracion()
        {
            try
            {
                var cs = GetConnectionString();
                using (var ctx = new GestorMerchandisingContext(cs))
                {
                    var database = ctx.Database.Connection.Database;
                    var server = ctx.Database.Connection.DataSource;

                    return $"Servidor: {server}\nBase de datos: {database}\nEstado: Conectado\nArquitectura: UI → BLL → DAL (EF invisible para UI)";
                }
            }
            catch (Exception ex)
            {
                return $"Error obteniendo configuración: {ex.Message}";
            }
        }

        /// <summary>
        /// Verifica si hay una configuración válida.
        /// </summary>
        public static bool VerificarConfiguracion()
        {
            return ServiceFactory.VerificarConfiguracion();
        }

        // =====================================================================
        // HELPER PRIVADO
        // =====================================================================

        private static string GetConnectionString()
        {
            return ServiceFactory.GetConnectionStringInterno();
        }
        private static string FullError(Exception ex)
        {
            var msg = ex.Message;
            var i = ex.InnerException;
            while (i != null) { msg += " | " + i.Message; i = i.InnerException; }
            return msg;
        }

        private static void EnsureDatabaseUpToDate()
        {
            var cs = ServiceFactory.GetConnectionStringInterno();
            DatabaseUpgrade.EnsureUpToDate(cs);
        }

        public static string DiagnosticoEsquema()
        {
            try
            {
                var cs = ServiceFactory.GetConnectionStringInterno();
                using (var cn = new SqlConnection(cs))
                using (var cmd = cn.CreateCommand())
                {
                    cn.Open();
                    cmd.CommandText = @"
SELECT name FROM sys.tables WHERE name IN ('Perfil','Usuario','Bitacora','Sesion');";
                    using (var r = cmd.ExecuteReader())
                    {
                        var found = new System.Collections.Generic.List<string>();
                        while (r.Read()) found.Add(r.GetString(0));
                        var target = new[] { "Perfil", "Usuario", "Bitacora", "Sesion" };
                        var faltan = string.Join(",", target.Except(found));
                        var estan = string.Join(",", found);
                        return "Core: [" + estan + "]" + (string.IsNullOrEmpty(faltan) ? " (OK)" : " | Faltan: " + faltan);
                    }
                }
            }
            catch (Exception ex) { return "Diag esquema falló: " + FullError(ex); }
        }
        private static string Sha256Hex(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private static string Sha256Base64(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
