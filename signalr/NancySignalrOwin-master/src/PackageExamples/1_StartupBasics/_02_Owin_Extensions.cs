namespace PackageExamples._1_StartupBasics
{
    using Owin;

    public class _02_Owin_Extensions
    {
        // Nuget package: Owin.Extensions
        public class Startup1
        {
            public void Configuration(IAppBuilder builder)
            {
                builder.Use((context, next) => context.Response.WriteAsync("Sup"));
            }
        }

        public class Startup2
        {
            public void Configuration(IAppBuilder builder)
            {
                builder.Use((context, next) =>
                                       {
                                           // write headers first!
                                           context.Response.StatusCode = 200; 
                                           context.Response.Write("Sup");
                                           return next();
                                       });
            }
        }
    }
}