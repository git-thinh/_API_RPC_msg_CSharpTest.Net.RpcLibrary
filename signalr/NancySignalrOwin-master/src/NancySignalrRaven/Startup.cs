namespace SampleApp
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin;
    using Microsoft.Owin.Diagnostics;
    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.StaticFiles;
    using Microsoft.Owin.StaticFiles.Infrastructure;
    using Owin;
    using Raven.Client;

    public class Startup
    {
        private readonly IDocumentStore _documentStore;

        public Startup(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Configuration(IAppBuilder builder)
        {
            // Configure SignalR DI
            var resolver = new DefaultDependencyResolver();
            resolver.Register(typeof(Chat), () => new Chat(_documentStore));
            var hubConfig = new HubConfiguration
            {
                Resolver = resolver
            };

            // Nancy DI container is configured in its bootstrapper
            var sampleBootstrapper = new SampleBootstrapper(_documentStore);

            // Note: it's perfectly viable to have an over-arching single application container
            // and configure signalr and nancy to use it.

            
            builder
                .Map("/fault",
                         faultBuilder => faultBuilder
                            .UseErrorPage(new ErrorPageOptions { ShowExceptionDetails = true })
                            .Use( (context, next) => { throw new Exception("oops!"); }))
                .Map("/files",
                    siteBuilder =>
                    {
                        siteBuilder.UseBasicAuthentication("nancy-signalr", (id, secret) =>
                        {
                            IEnumerable<Claim> claims = new[] {new Claim("id", id)};
                            return Task.FromResult(claims);
                        });

                        var options = new DirectoryBrowserOptions
                        {
                            FileSystem = new PhysicalFileSystem(@"c:\")
                        };
                        // .UseDenyAnonymous()
                        siteBuilder.UseDirectoryBrowser(options);
                    })
                .UseFileServer(new FileServerOptions
                {
                    RequestPath = new PathString("/scripts"),
                    FileSystem = new PhysicalFileSystem("scripts")
                })
                .Map("/site", siteBuilder => siteBuilder.UseNancy(cfg => cfg.Bootstrapper = sampleBootstrapper))
                .MapHubs(hubConfig)
                .UseWelcomePage();
        }
    }
}