using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UI.Localization;

namespace UI.Helpers
{
    /// <summary>
    /// Provee mensajes de error amigables a partir de excepciones técnicas.
    /// </summary>
    public static class ErrorMessageHelper
    {
        /// <summary>
        /// Códigos de error de SQL Server asociados a problemas de tiempo de espera.
        /// </summary>
        private static readonly HashSet<int> SqlTimeoutNumbers = new HashSet<int> { -2, 258 }; // -2 default timeout, 258 win32 timeout
        /// <summary>
        /// Códigos de error de SQL Server que indican indisponibilidad del servidor.
        /// </summary>
        private static readonly HashSet<int> SqlUnavailableNumbers = new HashSet<int> { 4060, 53, 18456, 17142, 64 };

        /// <summary>
        /// Obtiene un mensaje traducido y comprensible para una excepción determinada.
        /// </summary>
        /// <param name="exception">Excepción original capturada.</param>
        /// <returns>Mensaje destinado a mostrar al usuario final.</returns>
        public static string GetFriendlyMessage(Exception exception)
        {
            var info = ResolveErrorInfo(exception);
            var args = info.Arguments ?? Array.Empty<object>();
            return info.Key.Traducir(args);
        }

        /// <summary>
        /// Determina la clave de traducción y argumentos apropiados para la excepción recibida.
        /// </summary>
        /// <param name="exception">Excepción analizada.</param>
        /// <returns>Información con la clave de error y sus argumentos.</returns>
        private static ErrorInfo ResolveErrorInfo(Exception exception)
        {
            if (exception == null)
            {
                return new ErrorInfo("errors.unexpected", new object[] { "errors.noDetail".Traducir() });
            }

            foreach (var current in EnumerateExceptions(exception))
            {
                if (IsTimeoutException(current))
                {
                    return new ErrorInfo("errors.timeout");
                }

                if (IsDatabaseException(current))
                {
                    return new ErrorInfo("errors.databaseUnavailable");
                }

                if (IsExternalException(current))
                {
                    return new ErrorInfo("errors.external");
                }
            }

            var root = GetInnermostException(exception);
            var detail = root?.Message;
            if (string.IsNullOrWhiteSpace(detail))
            {
                detail = exception.Message;
            }

            if (string.IsNullOrWhiteSpace(detail))
            {
                detail = "errors.noDetail".Traducir();
            }
            else
            {
                var translated = detail.Traducir();
                if (!string.Equals(translated, detail, StringComparison.OrdinalIgnoreCase))
                {
                    detail = translated;
                }
            }

            return new ErrorInfo("errors.unexpected", new object[] { detail });
        }

        /// <summary>
        /// Recorre recursivamente la excepción y todas sus causas.
        /// </summary>
        /// <param name="exception">Excepción raíz.</param>
        /// <returns>Enumeración de excepciones encontradas.</returns>
        private static IEnumerable<Exception> EnumerateExceptions(Exception exception)
        {
            if (exception == null)
                yield break;

            yield return exception;

            if (exception is AggregateException aggregate)
            {
                foreach (var inner in aggregate.InnerExceptions ?? Enumerable.Empty<Exception>())
                {
                    foreach (var nested in EnumerateExceptions(inner))
                    {
                        yield return nested;
                    }
                }
            }
            else if (exception.InnerException != null)
            {
                foreach (var nested in EnumerateExceptions(exception.InnerException))
                {
                    yield return nested;
                }
            }
        }

        /// <summary>
        /// Busca la excepción más interna disponible para recuperar el detalle original.
        /// </summary>
        /// <param name="exception">Excepción desde la cual comenzar el análisis.</param>
        /// <returns>Excepción más específica encontrada.</returns>
        private static Exception GetInnermostException(Exception exception)
        {
            if (exception == null)
                return null;

            while (true)
            {
                if (exception is AggregateException aggregate)
                {
                    if (aggregate.InnerExceptions != null && aggregate.InnerExceptions.Count == 1)
                    {
                        exception = aggregate.InnerExceptions[0];
                        continue;
                    }

                    var first = aggregate.InnerExceptions?.FirstOrDefault();
                    return first ?? aggregate;
                }

                if (exception.InnerException == null)
                    return exception;

                exception = exception.InnerException;
            }
        }

        /// <summary>
        /// Determina si la excepción recibida está relacionada con un tiempo de espera.
        /// </summary>
        /// <param name="exception">Excepción a evaluar.</param>
        /// <returns><c>true</c> si corresponde a un timeout.</returns>
        private static bool IsTimeoutException(Exception exception)
        {
            if (exception == null)
                return false;

            if (exception is TimeoutException)
                return true;

            if (exception is TaskCanceledException taskCanceled && !taskCanceled.CancellationToken.CanBeCanceled)
                return true;

            if (exception is SqlException sqlException)
            {
                if (SqlTimeoutNumbers.Contains(sqlException.Number))
                    return true;
            }

            if (exception is DbException dbException)
            {
                if (ContainsTimeoutKeyword(dbException.Message))
                    return true;
            }

            return ContainsTimeoutKeyword(exception.Message);
        }

        /// <summary>
        /// Indica si el mensaje contiene palabras clave de timeout.
        /// </summary>
        /// <param name="message">Mensaje a analizar.</param>
        /// <returns>Verdadero si se detecta vocabulario relacionado con tiempo de espera.</returns>
        private static bool ContainsTimeoutKeyword(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return false;

            return message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0
                || message.IndexOf("tiempo de espera", StringComparison.OrdinalIgnoreCase) >= 0
                || message.IndexOf("tiempo expirado", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Determina si la excepción representa un problema de conexión o disponibilidad de base de datos.
        /// </summary>
        /// <param name="exception">Excepción evaluada.</param>
        /// <returns><c>true</c> cuando la base de datos no está disponible.</returns>
        private static bool IsDatabaseException(Exception exception)
        {
            if (exception == null)
                return false;

            if (exception is SqlException sqlException)
            {
                foreach (SqlError error in sqlException.Errors)
                {
                    if (SqlTimeoutNumbers.Contains(error.Number))
                        continue;

                    if (SqlUnavailableNumbers.Contains(error.Number))
                        return true;
                }

                if (ContainsDatabaseUnavailableKeyword(sqlException.Message))
                    return true;
            }

            if (exception is DbException dbException)
            {
                if (ContainsDatabaseUnavailableKeyword(dbException.Message))
                    return true;
            }

            var typeName = exception.GetType().FullName ?? string.Empty;
            if (typeName.StartsWith("System.Data.Entity", StringComparison.OrdinalIgnoreCase))
            {
                if (ContainsDatabaseUnavailableKeyword(exception.Message))
                    return true;
            }

            if (exception is InvalidOperationException invalidOperation)
            {
                var message = invalidOperation.Message ?? string.Empty;
                if (message.IndexOf("connection", StringComparison.OrdinalIgnoreCase) >= 0
                    || message.IndexOf("base de datos", StringComparison.OrdinalIgnoreCase) >= 0
                    || message.IndexOf("database", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Revisa si un mensaje contiene indicios de indisponibilidad de la base de datos.
        /// </summary>
        /// <param name="message">Mensaje a revisar.</param>
        /// <returns>Verdadero si el texto describe problemas para acceder a la base.</returns>
        private static bool ContainsDatabaseUnavailableKeyword(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return false;

            var lower = message.ToLowerInvariant();
            return lower.Contains("could not connect")
                || lower.Contains("no se pudo conectar")
                || lower.Contains("server was not found")
                || lower.Contains("no existe el servidor")
                || lower.Contains("the login failed")
                || lower.Contains("login failed")
                || lower.Contains("error de conexion")
                || lower.Contains("error de conexión")
                || lower.Contains("no se puede abrir la base de datos")
                || lower.Contains("cannot open database")
                || lower.Contains("timeout expired before obtaining a connection");
        }

        /// <summary>
        /// Determina si el error proviene de recursos externos como red o sistema operativo.
        /// </summary>
        /// <param name="exception">Excepción a analizar.</param>
        /// <returns><c>true</c> si se trata de un error externo.</returns>
        private static bool IsExternalException(Exception exception)
        {
            if (exception == null)
                return false;

            if (exception is IOException || exception is UnauthorizedAccessException)
                return true;

            if (exception is Win32Exception || exception is ExternalException)
                return true;

            if (exception is WebException || exception is SocketException)
                return true;

            var typeName = exception.GetType().FullName ?? string.Empty;
            if (typeName.IndexOf("System.Net", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// Estructura con la información necesaria para traducir un error.
        /// </summary>
        private readonly struct ErrorInfo
        {
            /// <summary>
            /// Crea una nueva instancia con la clave de traducción y sus argumentos.
            /// </summary>
            /// <param name="key">Clave de traducción a utilizar.</param>
            /// <param name="arguments">Argumentos opcionales para el mensaje.</param>
            public ErrorInfo(string key, object[] arguments = null)
            {
                Key = key;
                Arguments = arguments;
            }

            /// <summary>
            /// Clave de traducción asociada al error.
            /// </summary>
            public string Key { get; }
            /// <summary>
            /// Argumentos que se aplicarán al mensaje traducido.
            /// </summary>
            public object[] Arguments { get; }
        }
    }
}