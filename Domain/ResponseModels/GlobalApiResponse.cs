using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseModels;

public class GlobalApiResponse
{
    public bool isSuccess { get; set; }
    public string message { get; set; } = string.Empty;
    public object? data { get; set; }
    public string errorDetails { get; set; }

    public GlobalApiResponse(bool _isSuccess, string msg, object? dataObj = null, string? _errorDetails = null)
    {
        isSuccess = _isSuccess;
        message = msg;
        data = dataObj;
    }
}
