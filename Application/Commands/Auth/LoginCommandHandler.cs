using Domain.Abstraction;
using Domain.ResponseModels;
using Domain.ResponseModels.Auth;
using MediatR;
//using LiteMediator;

namespace Application.Commands.Auth;

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, GlobalApiResponse>
{
    public async Task<GlobalApiResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        string _token = JweTokenUtils.GenerateJWEToken();
        GlobalApiResponse response = new(true, "Login Successful", new LoginResponse { sessionToken = _token });
        return await Task.FromResult(response);
    }
}
