using Application.Commands.Auth;
using Domain.ResponseModels;
using MediatR;
//using LiteMediator;
using Microsoft.AspNetCore.Mvc;

namespace SampleApiApplication.Controllers;

[Route("api")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    public readonly IMediator _mediator = mediator;

    [HttpPost]
    [Route(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        GlobalApiResponse response = await _mediator.Send(command);
        return Ok(response);
    }
}
