namespace PackageExamples._2_Middleware
{
    using Owin;

    public class _05_StaticFiles
    {
        // Nuget package: Microsoft.Owin.StaticFiles
        public class Startup
        {
            public void Configuration(IAppBuilder builder)
            {
                builder.UseDirectoryBrowser(@"c:\");
            }
        }
    }
}