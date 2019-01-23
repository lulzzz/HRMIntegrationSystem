using NLog;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;
using System.Text;

namespace Shared.Logger.LayoutRenderers
{
    [LayoutRenderer("sticos-https")]
    public class HttpsLayoutRenderer : AspNetLayoutRendererBase
    {
        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
            var httpContext = HttpContextAccessor.HttpContext;
            builder.Append(httpContext?.Request.Scheme.ToLower() == "https" ? 1 : 0);
        }
    }
}
