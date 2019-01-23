using NLog;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;
using System.Text;

namespace Shared.Logger.LayoutRenderers
{
    [LayoutRenderer("sticos-port")]
    public class PortLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var httpContext = HttpContextAccessor.HttpContext;
            builder.Append(httpContext?.Request.Host.Port);
        }
    }
}
