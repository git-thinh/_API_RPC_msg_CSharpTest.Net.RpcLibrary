namespace PackageExamples._1_StartupBasics
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Owin;

    public class _01_TheStartup
    {
        // Named and shaped by convention
        // IAppBuilder comes from Owin nuget package, but can be a signature
        public class Startup
        {
            public void Configuration(IAppBuilder builder)
            {
                //builder.Use(object middleware, params object[] args)
                builder.Use(typeof (MyApp), new object[] {"foo"});
            }
        }

        public class MyApp
        {
            public MyApp(string bar) {}

            public Task Invoke(IDictionary<string, object> env)
            {
                var requestStream = (Stream) env["owin.RequestBody"];
                //...
                return Task.FromResult(0);
            }
        }
    }
}