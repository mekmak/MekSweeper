using System;

namespace MekSweeper.Logging.Interfaces
{
    public interface IMekLogger
    {
        IMekLogger SubLogger(string module);
        IMekLogger ForkLogger(string module);
        object Wrap(object value);

        void Trace(string message, params (string, object)[] tags);
        void Trace(string traceId, string message, params (string, object)[] tags);

        void Info(string message, params (string, object)[] tags);
        void Info(string traceId, string message, params (string, object)[] tags);
        void Info(string traceId, string message, Exception ex, params (string, object)[] tags);

        void Warn(string message, params (string, object)[] tags);
        void Warn(string traceId, string message, params (string, object)[] tags);
        void Warn(string traceId, string message, Exception ex, params (string, object)[] tags);

        void Error(string message, params (string, object)[] tags);
        void Error(string traceId, string message, params (string, object)[] tags);
        void Error(string traceId, string message, Exception ex, params (string, object)[] tags);
    }
}
