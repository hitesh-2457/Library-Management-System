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
    [Route("api/Publisher")]
    public class PublisherController : Controller
    {
        // GET api/values
        [HttpGet]
        public List<Publisher> Get()
        {
            using (var ctx = new libraryContext())
            {
                return new PublisherRepository(ctx).GetPublishers().ToList();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Publisher Get(int id)
        {
            using (var ctx = new libraryContext())
            {
                return new PublisherRepository(ctx).GetById(id).FirstOrDefault();
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string name)
        {
            using (var ctx = new libraryContext())
            {
                new PublisherRepository(ctx).Add(name);
                ctx.SaveChanges();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody]string name)
        {
            using (var ctx = new libraryContext())
            {
                var result = new PublisherRepository(ctx).Update(id, name);
                if (result == string.Empty)
                    ctx.SaveChanges();
                return result;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var ctx = new libraryContext())
            {
                new PublisherRepository(ctx).DeleteById(id);
                ctx.SaveChanges();
            }
        }
    }
}