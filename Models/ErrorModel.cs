using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Models
{
    public class ErrorModel
    {
        public string Error { get; set; }
        public int RequestedTimes { get; set; } = 1;
        public string ErrorType { private get; set; }

        public string GetErrorType()
        {
            return ErrorType;
        }
    }
}
