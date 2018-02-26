namespace PackageExamples._1_StartupBasics
{
    using System;
    using Microsoft.Owin.Hosting;
    using Owin;

    // As of Katana v2.0, Dependency injection support for Startups are not supported
    public class _03_DependencyInjection
    {
        public interface IMyService
        {
            string GetFoo();
        }

        public class MyService : IMyService
        {
            public string GetFoo()
            {
                return "Bar";
            }
        }

        public class Startup
        {
            private readonly IMyService _myService;

            public Startup(IMyService myService)
            {
                _myService = myService;
            }

            public void Configuration(IAppBuilder builder)
            {
                builder.Use((context, next) => context.Response.WriteAsync(_myService.GetFoo()));
            }
        }

        public class Program
        {
            public static void Main()
            {
                using (WebApp.Start("http://+:8080", app => new Startup(new MyService()).Configuration(app))) 
                {
                    Console.ReadLine();
                }
            }
        }
    }
}