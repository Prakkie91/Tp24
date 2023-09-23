using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tp24.Application.Features.Receivable.Queries;
using IResult = Tp24.Common.Wrappers.IResult;
using System.Collections.Generic;
using Tp24.Application.Features.Receivable.Commands;
using Tp24.Application.Features.Receivable.Responses;
using Tp24.Common.Wrappers;

namespace Tp24.Api.Controllers.v1;

[Route("api/v1/receivable")]
public class ReceivableController : BaseApiController<ReceivableController>
{
    public ReceivableController(IMediator mediator, ILogger<ReceivableController> logger)
        : base(mediator, logger)
    {
    }

    /// <summary>
    ///     Retrieves summary statistics about the stored receivables data.
    /// </summary>
    /// <returns>Summary statistics about open and closed invoices.</returns>
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult<ReceivableSummaryResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IResult))]
    public async Task<IActionResult> GetReceivablesSummary()
    {
        var query = new GetReceivableSummaryQuery();
        var result = await Mediator.Send(query);
        if (result.Succeeded) return Ok(result);
        return BadRequest(result);
    }

    /// <summary>
    ///     Accepts a payload containing receivables data and stores it.
    /// </summary>
    /// <param name="request">The command model containing the receivable details.</param>
    /// <returns>Confirmation of the stored data.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IResult))]
    public async Task<IActionResult> AddReceivable([FromBody] AddReceivablesCommand request)
    {
        var result = await Mediator.Send(request);
        if (result.Succeeded) return Ok(result);
        return BadRequest(result);
    }
}
