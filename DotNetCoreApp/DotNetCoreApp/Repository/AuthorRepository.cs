using DotNetCoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetCoreApp.Repository
{
    public class AuthorRepository
    {
        private libraryContext context;
        public AuthorRepository() { this.context = new libraryContext(); }

        public AuthorRepository(libraryContext ctx) { this.context = ctx; }

        public IEnumerable<Authors> GetAuthors() { return context.Authors.AsEnumerable(); }

        public IEnumerable<Authors> GetById(int id) { return GetAuthors().Where(x => x.AuthorId == id); }

        public IEnumerable<Authors> GetByName(string name) { return GetAuthors().Where(x => x.Name.Equals(name)); }

        public void Add(Authors Author) { context.Authors.Add(Author); }

        public void Add(string name)
        {
            Authors author = new Authors();
            author.Name = name;
            Add(author);
        }

        public void Update(int id, string name)
        {
            var pub = GetById(id).FirstOrDefault();
            pub.Name = name;
        }

        public void Delete(Authors auth) { context.Authors.Remove(auth); }

        public void DeleteById(int id) { Delete(GetById(id).FirstOrDefault()); }

        public void DeleteByName(string name) { Delete(GetByName(name).FirstOrDefault()); }
    }
}