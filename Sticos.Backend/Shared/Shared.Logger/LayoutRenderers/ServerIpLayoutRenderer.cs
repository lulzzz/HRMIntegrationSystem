using Microsoft.AspNetCore.Http.Features;
using NLog;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;
using System.Text;

namespace Shared.Logger.LayoutRenderers
{
    [LayoutRenderer("sticos-server-ip")]
    public class ServerIpLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var httpContext = HttpContextAccessor.HttpContext;
            var httpConnectionFeature = httpContext?.Features.Get<IHttpConnectionFeature>();
            builder.Append(httpConnectionFeature?.LocalIpAddress);
        }
    }
}
