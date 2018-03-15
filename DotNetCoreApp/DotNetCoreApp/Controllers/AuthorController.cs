using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreApp.Models;
using DotNetCoreApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Author")]
    public class AuthorController : Controller
    {
        // GET: api/Author
        [HttpGet]
        public List<Authors> Get()
        {
            using (var ctx = new libraryContext())
            {
                return new AuthorRepository(ctx).GetAuthors().ToList();
            }
        }

        // GET: api/Author/5
        [HttpGet("{id}")]
        public Authors Get(int id)
        {
            using (var ctx = new libraryContext())
            {
                return new AuthorRepository(ctx).GetById(id).FirstOrDefault();
            }
        }

        // POST: api/Author
        [HttpPost]
        public void Post([FromBody]string name)
        {
            using (var ctx = new libraryContext())
            {
                new AuthorRepository(ctx).Add(name);
                ctx.SaveChanges();
            }
        }

        // PUT: api/Author/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string name)
        {
            using (var ctx = new libraryContext())
            {
                new AuthorRepository(ctx).Update(id, name);
                ctx.SaveChanges();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var ctx = new libraryContext())
            {
                new AuthorRepository(ctx).DeleteById(id);
                ctx.SaveChanges();
            }
        }
    }
}
