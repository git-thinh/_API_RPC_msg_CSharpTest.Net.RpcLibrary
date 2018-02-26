namespace SampleApp
{
    using System;
    using Microsoft.Owin.Hosting;
    using Raven.Client;
    using Raven.Client.Embedded;

    internal class Program
    {
        private static void Main(string[] args)
        {
            using (IDocumentStore documentStore = new EmbeddableDocumentStore {RunInMemory = true}.Initialize())
            {
                using (WebApp.Start("http://+:2020", app => new Startup(documentStore).Configuration(app)))
                {
                    Console.WriteLine("Started on port 2020");
                    Console.ReadKey();
                    Console.WriteLine("Stopping");
                }
            }
        }
    }
}