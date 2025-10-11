using Domain.RequestModels.Auth;
using Domain.ResponseModels;
using MediatR;
//using LiteMediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Auth;

public sealed class LoginCommand : LoginRequest, IRequest<GlobalApiResponse>
{
}
