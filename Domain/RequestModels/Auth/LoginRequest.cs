using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RequestModels.Auth;

public class LoginRequest
{
    public required string userName { get; set; }
    public required string password { get; set; }
}
