using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstraction;

public static class ExtensionMethods
{
    public static string ExceptionMessage(this Exception ex)
    {
        string errMsg = ex.InnerException?.Message ?? ex.Message;
        return errMsg;
    }
}
