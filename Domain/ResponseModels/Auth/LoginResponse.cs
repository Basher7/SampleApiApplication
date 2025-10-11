using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ResponseModels.Auth;

public class LoginResponse
{
    public required string sessionToken { get; set; }
}
