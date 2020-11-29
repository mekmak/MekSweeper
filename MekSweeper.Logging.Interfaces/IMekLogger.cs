using System;

namespace MekSweeper.Logging.Interfaces
{
    public interface IMekLogger
    {
        IMekLogger SubLogger(string module);
        object Wrap(object value);

        void Trace(string method, params (string, object)[] tags);
        void Trace(string traceId, string method, params (string, object)[] tags);

        void Info(string method, params (string, object)[] tags);
        void Info(string traceId, string method, params (string, object)[] tags);
        void Info(string traceId, string method, Exception ex, params (string, object)[] tags);

        void Warn(string method, params (string, object)[] tags);
        void Warn(string traceId, string method, params (string, object)[] tags);
        void Warn(string traceId, string method, Exception ex, params (string, object)[] tags);

        void Error(string method, params (string, object)[] tags);
        void Error(string traceId, string method, params (string, object)[] tags);
        void Error(string traceId, string method, Exception ex, params (string, object)[] tags);
    }
}
