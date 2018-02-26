namespace PackageExamples._2_Middleware
{
    using System.Threading.Tasks;
    using Owin;

    public class _06_MappingRouting
    {
        // Nuget package: Microsoft.Owin.Mapping 
      
        public void Configuration(IAppBuilder builder)
        {
            builder
                .Map("/api", b => b.Run(context=>
                {
                    context.Response.StatusCode = 302;
                    return Task.FromResult(0);
                }))
                .Map("/admin", b => { /* etc */})
                .MapWhen(env => true, b => { /* etc */});
        }
    }
}