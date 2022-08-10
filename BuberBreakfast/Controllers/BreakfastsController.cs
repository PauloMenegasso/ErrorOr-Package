using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfast;
using Microsoft.AspNetCore.Mvc;

namespace BuberBreakfast.Controllers;

public class BreakfastsController : ApiController
{
    private readonly IBreakfastService breakfastService;
    public BreakfastsController(IBreakfastService breakfastService)
    {
        this.breakfastService = breakfastService;
    }

    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        var initializeBreakfastResult = Breakfast.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet
        );

        if (initializeBreakfastResult.IsError)
        {
            return Problem(initializeBreakfastResult.Errors);
        }

        var breakfast = initializeBreakfastResult.Value;
        var createBreakfastResult = this.breakfastService.CreateBreakfast(breakfast);

        return createBreakfastResult.Match(
            created => CreatedAtGetBreakfast(breakfast),
            errors => Problem(errors)
        );
    }

    [HttpGet]
    public IActionResult GetAllBreakfasts()
    {
        return Ok(this.breakfastService.GetAllBreakfasts());
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        var getBreakfastResult = this.breakfastService.GetBreakfast(id);

        return getBreakfastResult.Match(
            breakfast => Ok(MapToResponse(breakfast)),
            errors => Problem(errors)
        );
    }


    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {
        var initializeBreakfastResult = Breakfast.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet,
            id
        );

        if (initializeBreakfastResult.IsError)
        {
            return Problem(initializeBreakfastResult.Errors);
        }

        var breakfast = initializeBreakfastResult.Value;
        var upsertResult = this.breakfastService.UpsertBreakfast(breakfast);

        return upsertResult.Match(
            upserted => upserted.IsNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        var deleteResult = this.breakfastService.DeleteBreakfast(id);

        return deleteResult.Match(
            deleted => NoContent(),
            errors => Problem(errors)
        );
    }

    private static BreakfastResponse MapToResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet
        );
    }

    private CreatedAtActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: MapToResponse(breakfast)
        );
    }
}
