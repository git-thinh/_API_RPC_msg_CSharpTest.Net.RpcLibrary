namespace PackageExamples.Temp
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MyOwinApp
    {
        public Task Invoke(IDictionary<string, object> environment)
        {
            string requestPath = (string)environment["owin.RequestPath"];
            //...etc
            return Task.FromResult(0);
        }
    }
}