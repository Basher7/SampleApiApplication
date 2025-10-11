using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstraction;

public sealed class MessageCollections
{
    public static string Success => "Success";
    public static string InvalidSession => "Invalid session token, Please Login Again";
}
