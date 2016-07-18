using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangentIdeas.Core.Common.Exceptions
{
    public class GeneralSystemException : Exception
    {
        public string ExceptionCode { get; set; }

        public override string Message => $"{ExceptionCode} - {base.Message}";

        public GeneralSystemException(string exceptionCode)
        {
            ExceptionCode = exceptionCode;
        }
    }
}
