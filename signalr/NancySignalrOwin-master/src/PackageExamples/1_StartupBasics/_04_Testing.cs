namespace PackageExamples._1_StartupBasics
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Owin.Testing;
    using Owin;
    using Xunit;

    public class _04_Testing
    {
        // Slide
        public class Startup
        {
            public void Configuration(IAppBuilder builder)
            {
                builder.Use((context, next) =>
                {
                    context.Response.StatusCode = 404;
                    return Task.FromResult(0);
                });
            }
        }

        public class WebAppTests
        {
            [Fact]
            public async Task Should_get_NotFound()
            {
                // Nuget package: Microsoft.Owin.Testing
                TestServer testServer = TestServer.Create(builder => new Startup()
                    .Configuration(builder));
                HttpResponseMessage response = await testServer
                    .HttpClient.GetAsync("http://localhost");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact]
            public async Task Should_get_NotFound2()
            {
                // Nuget package: Microsoft.Owin.Testing
                TestServer testServer = TestServer.Create(builder => new Startup()
                    .Configuration(builder));
                HttpResponseMessage response = await testServer
                    .HttpClient.GetAsync("http://localhost");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}