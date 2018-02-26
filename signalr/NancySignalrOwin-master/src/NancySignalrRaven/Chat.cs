namespace SampleApp
{
    using System;
    using Microsoft.AspNet.SignalR;
    using Raven.Client;

    public class Chat : Hub
    {
        private readonly IDocumentStore _documentStore;

        public Chat(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Send(string message)
        {
            Clients.All.addMessage(message);
            using (IDocumentSession session = _documentStore.OpenSession())
            {
                session.Store(new ChatLogDocument {Created = DateTime.UtcNow, Message = message});
                session.SaveChanges();
            }
        }
    }
}