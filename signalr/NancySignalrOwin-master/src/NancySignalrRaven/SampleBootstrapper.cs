namespace SampleApp
{
    using Nancy;
    using Nancy.TinyIoc;
    using Raven.Client;

    public class SampleBootstrapper : DefaultNancyBootstrapper
    {
        private readonly IDocumentStore _documentStore;

        public SampleBootstrapper(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(_documentStore);
        }
    }
}