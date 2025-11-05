using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Abstraction;

public static class ExtensionMethods
{
    public static string ExceptionMessage(this Exception ex)
    {
        string errMsg = ex.InnerException?.Message ?? ex.Message;
        return errMsg;
    }


    /// <summary>
    /// Convert any object to Json string
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJsonString(this object obj)
    {
        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = null,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        string jsonStr = JsonSerializer.Serialize(obj, options);
        return jsonStr;
    }

}
