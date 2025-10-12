using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseModels;

public class GlobalApiResponse(bool _isSuccess, string msg, object? dataObj = null, string? _errorDetails = null)
{
    public bool isSuccess { get; set; } = _isSuccess;
    public string message { get; set; } = msg;
    public object? data { get; set; } = dataObj;
    public string? errorDetails { get; set; } = _errorDetails;
}
