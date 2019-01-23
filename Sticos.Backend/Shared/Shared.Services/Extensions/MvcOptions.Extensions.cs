using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Linq;

namespace Shared.Services.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void UseCustomerIdRoutePrefix(this MvcOptions opts, Type[] ignoredControllers)
        {
            opts.Conventions.Insert(0, new RouteConvention(new RouteAttribute("{customerId}"), ignoredControllers));
        }
    }

    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;
        private readonly Type[] _ignoredControllers;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider, Type[] ignoredControllers)
        {
            _centralPrefix = new AttributeRouteModel(routeTemplateProvider);
            _ignoredControllers = ignoredControllers;
        }
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                if (_ignoredControllers != null && _ignoredControllers.Contains(controller.ControllerType))
                {
                    continue;
                }

                var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectorModel in matchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix, selectorModel.AttributeRouteModel);
                    }
                }

                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = _centralPrefix;
                    }
                }
            }
        }
    }
}
