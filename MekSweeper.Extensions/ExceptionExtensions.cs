using System;
using System.Collections.Generic;

namespace MekSweeper.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ShortSummary(this Exception ex)
        {
            if (ex == null)
            {
                return "";
            }

            var messages = new List<string>();
            Exception currentException = ex;
            while (currentException != null)
            {
                messages.Add(currentException.Message);
                currentException = currentException.InnerException;
            }

            return string.Join(" -> ", messages);
        }

        public static string FullSummary(this Exception ex)
        {
            if (ex == null)
            {
                return "";
            }

            var messages = new List<string>();
            Exception currentException = ex;
            while (currentException != null)
            {
                messages.Add(currentException.Message);
                messages.Add(currentException.StackTrace);

                currentException = currentException.InnerException;
            }

            return string.Join(Environment.NewLine, messages);
        }
    }
}
