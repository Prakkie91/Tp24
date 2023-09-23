using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tp24.Api.Filters;

namespace Tp24.Api.Controllers;

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
[ApiController]
[Produces("application/json")]
[ValidateApiModelState]
public abstract class BaseApiController<T> : ControllerBase
{
    /// <summary>
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected BaseApiController(IMediator mediator, ILogger logger)
    {
        Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Provides mediator instance to derived controllers for handling CQRS operations.
    /// </summary>
    protected IMediator Mediator { get; }

    /// <summary>
    ///     Provides logger instance to derived controllers for logging capabilities.
    /// </summary>
    protected ILogger Logger { get; }
}