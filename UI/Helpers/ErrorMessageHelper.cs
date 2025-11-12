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
    public static class ErrorMessageHelper
    {
        private static readonly HashSet<int> SqlTimeoutNumbers = new HashSet<int> { -2, 258 }; // -2 default timeout, 258 win32 timeout
        private static readonly HashSet<int> SqlUnavailableNumbers = new HashSet<int> { 4060, 53, 18456, 17142, 64 };

        public static string GetFriendlyMessage(Exception exception)
        {
            var info = ResolveErrorInfo(exception);
            var args = info.Arguments ?? Array.Empty<object>();
            return info.Key.Traducir(args);
        }

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

        private static bool ContainsTimeoutKeyword(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return false;

            return message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0
                || message.IndexOf("tiempo de espera", StringComparison.OrdinalIgnoreCase) >= 0
                || message.IndexOf("tiempo expirado", StringComparison.OrdinalIgnoreCase) >= 0;
        }

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

        private readonly struct ErrorInfo
        {
            public ErrorInfo(string key, object[] arguments = null)
            {
                Key = key;
                Arguments = arguments;
            }

            public string Key { get; }
            public object[] Arguments { get; }
        }
    }
}