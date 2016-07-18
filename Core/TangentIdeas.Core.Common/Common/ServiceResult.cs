using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangentIdeas.Core.Common.Common
{
    public class ServiceResult
    {
        public string Message { get; set; }
        public object Data { get; set; }
        public string ExceptionCode { get; set; }
        public Exception Exception { get; set; }
        public ServiceResultType ServiceResultType { get; set; }

        public ServiceResult()
        {
        }

        public ServiceResult(ServiceResultType resultType)
        {
            ServiceResultType = resultType;
        }
    }

    public class ServiceResult<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
        public string ExceptionCode { get; set; }
        public Exception Exception { get; set; }
        public ServiceResultType ServiceResultType { get; set; }
    }

    public enum ServiceResultType
    {
        Fail = 0,
        Success = 1,
        Warning = 2,
        NotKnown = 3
    }

    public static class ServiceMessage
    {
        public const string UserNameIsUsed = "Kullanıcı Adı Kullanımda";
        public const string EmailIsUsed = "Email adresi Kullanımda";
    }
}
