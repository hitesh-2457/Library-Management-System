using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreApp.Models;
using DotNetCoreApp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Books")]
    public class BooksController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            using (var ctx = new libraryContext())
            {
                return new BooksRepository(ctx).GetBooks();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Book Get(string id)
        {
            using (var ctx = new libraryContext())
            {
                return new BooksRepository(ctx).GetByIsbn(id).FirstOrDefault();
            }
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]string isbn, string title, string cover, int pages, string publisher, string[] authors)
        {
            using (var ctx = new libraryContext())
            {
                var result = new BooksRepository(ctx).Add(isbn, title, cover, pages, publisher, authors);
                if (result == string.Empty)
                    ctx.SaveChanges();
                return result;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public string Put(int id, [FromBody]string isbn, string title, string cover, int pages, string publisher, string[] authors)
        {
            using (var ctx = new libraryContext())
            {
                var result = new BooksRepository(ctx).Update(isbn, title, cover, pages, publisher, authors);
                if (result == string.Empty)
                    ctx.SaveChanges();
                return result;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string isbn)
        {
            using (var ctx = new libraryContext())
            {
                new BooksRepository(ctx).DeleteByIsbn(isbn);
                ctx.SaveChanges();
            }
        }

        [Route("search/{search}")]
        public JsonResult GetSearch(string search)
        {
            using (var ctx = new libraryContext())
            {
                return Json(new BooksRepository(ctx).Search(search));
            }
        }
    }
}
