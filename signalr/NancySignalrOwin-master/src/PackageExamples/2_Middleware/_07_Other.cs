namespace PackageExamples._2_Middleware
{
    using Owin;

    public class _07_Other
    {
        // Nuget package: Microsoft.Owin.Diagnostics, Microsft.Owin.Compressions
        // UPDATE: Compressions package not available

        public void Configuration(IAppBuilder builder)
        {
            //builder
               // .UseStaticCompression();
               // .UseDiagnosticsPage("_diag");
        }
    }
}