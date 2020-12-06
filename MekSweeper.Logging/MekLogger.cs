using System;
using System.Collections.Generic;
using System.Linq;
using MekSweeper.Extensions;
using MekSweeper.Logging.Interfaces;

namespace MekSweeper.Logging
{
    public class MekLogger : IMekLogger
    {
        private readonly string _module;
        private readonly string _moduleId;

        public MekLogger(string module)
        {
            _module = module ?? "UNKNOWN";
            _moduleId = NewModuleId();
        }

        internal MekLogger(string parentModuleId, string module)
        {
            _module = module ?? "UNKNOWN";
            _moduleId = NewModuleId(parentModuleId);
        }

        public IMekLogger SubLogger(string module)
        {
            return new MekLogger(_moduleId, $"{_module}.{module}");
        }

        public IMekLogger ForkLogger(string module)
        {
            return new MekLogger(_moduleId, module);
        }

        public object Wrap(object value)
        {
            return $"[{value}]";
        }

        public void Trace(string message, params (string, object)[] tags)
        {
            Trace(null, message, tags);
        }

        public void Trace(string traceId, string message, params (string, object)[] tags)
        {
            Log4NetLogger.Trace(BuildMessage(traceId, message, null, false, tags));
        }

        public void Info(string message, params (string, object)[] tags)
        {
            Info(null, message, tags);
        }

        public void Info(string traceId, string message, params (string, object)[] tags)
        {
            Info(traceId, message, null, tags);
        }

        public void Info(string traceId, string message, Exception ex, params (string, object)[] tags)
        {
            Log4NetLogger.Info(BuildMessage(traceId, message, ex, false, tags));
        }

        public void Warn(string message, params (string, object)[] tags)
        {
            Warn(null, message, tags);
        }

        public void Warn(string traceId, string message, params (string, object)[] tags)
        {
            Warn(traceId, message, null, tags);
        }

        public void Warn(string traceId, string message, Exception ex, params (string, object)[] tags)
        {
            Log4NetLogger.Warn(BuildMessage(traceId, message, ex, false, tags));
        }

        public void Error(string message, params (string, object)[] tags)
        {
            Error(null, message, tags);
        }

        public void Error(string traceId, string message, params (string, object)[] tags)
        {
            Error(traceId, message, null, tags);
        }

        public void Error(string traceId, string message, Exception ex, params (string, object)[] tags)
        {
            Log4NetLogger.Error(BuildMessage(traceId, message, ex, true, tags));
        }

        private string BuildMessage(string traceId, string message, Exception ex, bool writeFullStackTrace, params (string, object)[] tags)
        {
            var messageTags = new List<(string, object)>
            {
                ("module", _module),
                ("message", message), 
                ("moduleId", _moduleId)
            };

            if (traceId != null)
            {
                messageTags.Add(("traceId", traceId));
            }

            messageTags.AddRange(tags ?? new (string, object)[0]);

            if (ex != null)
            {
                messageTags.Add(("exception", Wrap(writeFullStackTrace ? ex.FullSummary() : ex.ShortSummary())));
            }

            return string.Join(", ", messageTags.Select(tuple => $"{tuple.Item1}={tuple.Item2}"));
        }

        private string NewModuleId()
        {
            return Guid.NewGuid().ToString().Split("-")[0];
        }

        private string NewModuleId(string parentId)
        {
            return $"{parentId}.{NewModuleId()}";
        }
    }
}
