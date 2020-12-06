using System;

namespace MekSweeper.Logging.Interfaces
{
    public static class TraceId
    {
        public static string New()
        {
            return Guid.NewGuid().ToString().Split('-')[0];
        }

        public static string New(string parentTraceId)
        {
            return $"{parentTraceId}.{New()}";
        }
    }
}
