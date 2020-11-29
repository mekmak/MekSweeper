using System;
using System.Collections.Generic;
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

        public object Wrap(object value)
        {
            return $"[{value}]";
        }

        public void Trace(string method, params (string, object)[] tags)
        {
            Trace(null, method, tags);
        }

        public void Trace(string traceId, string method, params (string, object)[] tags)
        {
            Log4NetLogger.Trace(BuildMessage(traceId, method, null, false, tags));
        }

        public void Info(string method, params (string, object)[] tags)
        {
            Info(null, method, tags);
        }

        public void Info(string traceId, string method, params (string, object)[] tags)
        {
            Info(traceId, method, null, tags);
        }

        public void Info(string traceId, string method, Exception ex, params (string, object)[] tags)
        {
            Log4NetLogger.Info(BuildMessage(traceId, method, ex, false, tags));
        }

        public void Warn(string method, params (string, object)[] tags)
        {
            Warn(null, method, tags);
        }

        public void Warn(string traceId, string method, params (string, object)[] tags)
        {
            Warn(traceId, method, null, tags);
        }

        public void Warn(string traceId, string method, Exception ex, params (string, object)[] tags)
        {
            Log4NetLogger.Warn(BuildMessage(traceId, method, ex, false, tags));
        }

        public void Error(string method, params (string, object)[] tags)
        {
            Error(null, method, tags);
        }

        public void Error(string traceId, string method, params (string, object)[] tags)
        {
            Error(traceId, method, null, tags);
        }

        public void Error(string traceId, string method, Exception ex, params (string, object)[] tags)
        {
            Log4NetLogger.Error(BuildMessage(traceId, method, ex, true, tags));
        }

        private string BuildMessage(string traceId, string method, Exception ex, bool writeFullStackTrace, params (string, object)[] tags)
        {
            var messageTags = new List<(string, object)>
            {
                ("method", method), 
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

            return string.Join(", ", messageTags);
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
