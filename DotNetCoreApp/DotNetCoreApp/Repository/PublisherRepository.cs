using DotNetCoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetCoreApp.Repository
{
    public class PublisherRepository
    {
        private libraryContext context;
        public PublisherRepository() { this.context = new libraryContext(); }

        public PublisherRepository(libraryContext ctx) { this.context = ctx; }

        public IEnumerable<Publisher> GetPublishers() { return context.Publisher.AsEnumerable(); }

        public IEnumerable<Publisher> GetById(int id) { return GetPublishers().Where(x => x.Id == id); }

        public IEnumerable<Publisher> GetByName(string name) { return GetPublishers().Where(x => x.Name.Equals(name)); }

        public void Add(Publisher publisher) { context.Publisher.Add(publisher); }

        public void Add(string name)
        {
            Publisher publisher = new Publisher();
            publisher.Name = name;
            Add(publisher);
        }

        public string Update(int id, string name)
        {
            var pub = GetById(id).FirstOrDefault();
            if (pub == null) return "Publisher not found.";
            pub.Name = name;
            return string.Empty;
        }

        public void Delete(Publisher pub) { context.Publisher.Remove(pub); }

        public void DeleteById(int id) { Delete(GetById(id).FirstOrDefault()); }

        public void DeleteByName(string name) { Delete(GetByName(name).FirstOrDefault()); }
    }
}