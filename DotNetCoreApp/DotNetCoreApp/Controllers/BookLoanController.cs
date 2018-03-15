using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreApp.Models;
using DotNetCoreApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNetCoreApp.Controllers
{
    [Produces("application/json")]
    [Route("api/BookLoan")]
    public class BookLoanController : Controller
    {
        // GET: api/BookLoan
        [HttpGet]
        public JsonResult Get()
        {
            using (var ctx = new libraryContext())
            {
                var data = new BookLoanRepository(ctx).GetBookLoans().ToList();
                return Json(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
        }

        // GET: api/BookLoan/5
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(int id)
        {
            using (var ctx = new libraryContext())
            {
                return Json(new BookLoanRepository(ctx).GetById(id).ToList(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
        }

        // POST: api/BookLoan
        [HttpPost]
        public JsonResult Post([FromBody]BookLoans loan)
        {
            using (var ctx = new libraryContext())
            {
                var BkLnRepo = new BookLoanRepository(ctx);
                var result = BkLnRepo.Add(loan);
                if (result == null)
                {
                    ctx.SaveChanges();
                    return Json(new { message = "Success" });
                }
                else
                {
                    return Json(new { error = result });
                }

            }
        }

        // PUT: api/BookLoan/5
        [Route("checkin/{id}")]
        [HttpPut("{id}")]
        public void Put(int id)
        {
            using (var ctx = new libraryContext())
            {
                new BookLoanRepository(ctx).Update(id);
                ctx.SaveChanges();
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("search/{search}")]
        public JsonResult GetSearch(string search)
        {
            using (var ctx = new libraryContext())
            {
                return Json(new BookLoanRepository(ctx).Search(search), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
        }
    }
}
