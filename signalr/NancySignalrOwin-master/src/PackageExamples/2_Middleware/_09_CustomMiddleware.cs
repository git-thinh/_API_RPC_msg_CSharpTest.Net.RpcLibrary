namespace PackageExamples._2_Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Owin;

    public class _09_CustomMiddleware
    {
        public class Startup
        {
            public void Configuration(IAppBuilder builder)
            {
                builder.Map("/admin", adminBuilder => adminBuilder
                                            .DenyNonLocalRequests()
                                            .Run(context =>
                    {
                        context.Response.Write("You're in.");
                        return Task.FromResult(0);
                    }));
            }
        }

        public class DenyNonLocalRequestsMiddleware
        {
            private readonly Func<IDictionary<string, object>, Task> _next;

            public DenyNonLocalRequestsMiddleware(Func<IDictionary<string, object>, Task> next)
            {
                _next = next;
            }

            public Task Invoke(IDictionary<string, object> env)
            {
                var context = new OwinContext(env);
                if (!Equals(IPAddress.Parse(context.Request.RemoteIpAddress), IPAddress.Loopback))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    return Task.FromResult(0);
                }
                return _next(env);
            }
        }
    }

    public static class OwinExtensions
    {
        public static IAppBuilder DenyNonLocalRequests(this IAppBuilder appBuilder)
        {
            return appBuilder.Use<_09_CustomMiddleware.DenyNonLocalRequestsMiddleware>();
        }
    }
}