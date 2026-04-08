using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Booking.API.Conventions;

/// <summary>
/// Applies a global route prefix to all controllers.
/// Eliminates the need to repeat "api/" in every [Route] attribute.
/// </summary>
public class GlobalRoutePrefixConvention : IApplicationModelConvention
{
    private readonly AttributeRouteModel _routePrefix;

    public GlobalRoutePrefixConvention(string prefix)
    {
        _routePrefix = new AttributeRouteModel(new RouteAttribute(prefix));
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            var matchedSelectors = controller.Selectors
                .Where(x => x.AttributeRouteModel is not null)
                .ToList();

            foreach (var selector in matchedSelectors)
            {
                selector.AttributeRouteModel = AttributeRouteModel
                    .CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
            }

            var unmatchedSelectors = controller.Selectors
                .Where(x => x.AttributeRouteModel is null)
                .ToList();

            foreach (var selector in unmatchedSelectors)
            {
                selector.AttributeRouteModel = _routePrefix;
            }
        }
    }
}
