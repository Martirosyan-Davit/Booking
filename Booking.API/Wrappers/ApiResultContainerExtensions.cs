using Microsoft.AspNetCore.Mvc;

namespace Booking.API.Wrappers;

public static class ApiResultContainerExtensions
{
    public static ActionResult<ApiResultContainer<T>> Success<T>(this ControllerBase controller, T data)
    {
        return new OkObjectResult(new ApiResultContainer<T>
        {
            Data = data
        });
    }

    public static ActionResult<ApiResultContainer<T>> Created<T>(this ControllerBase controller, T data)
    {
        return new ObjectResult(new ApiResultContainer<T>
        {
            Data = data
        })
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    public static ActionResult<ApiResultContainer> Success(this ControllerBase controller)
    {
        return new AcceptedResult();
    }
}
