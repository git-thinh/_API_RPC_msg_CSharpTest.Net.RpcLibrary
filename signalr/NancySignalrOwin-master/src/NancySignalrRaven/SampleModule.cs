namespace SampleApp
{
    using System.Dynamic;
    using System.Linq;
    using Nancy;
    using Raven.Client;
    using Raven.Client.Linq;

    public class SampleModule : NancyModule
    {
        public SampleModule(IDocumentStore documentStore)
        {
            Get["/"] = ctx => View["Index.html"];
            Get["/chat"] = ctx => View["chat.html"];
            Get["/logs"] = ctx =>
                           {
                               dynamic model = new ExpandoObject();
                               using (IDocumentSession session = documentStore.OpenSession())
                               {
                                   model.Logs = session
                                       .Query<ChatLogDocument>()
                                       .OrderBy(d => d.Created)
                                       .ToArray();
                               }
                               return View["ChatLog.cshtml", model];
                           };
        }
    }
}