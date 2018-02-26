namespace PackageExamples._2_Middleware
{
    using Owin;

    public class _08_Auth
    {
        // Nuget package: Microsoft.Owin.Auth.Basic

        public class Startup
        {
            public void Configuration(IAppBuilder builder)
            {
                //.UseBasicAuth((username, password) => Task.FromResult(false));
                // Package no longer available
           
            }
        }
    }
}