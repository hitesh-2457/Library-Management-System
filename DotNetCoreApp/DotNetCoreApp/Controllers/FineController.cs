using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreApp.Models;
using DotNetCoreApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotNetCoreApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Fine")]
    public class FineController : Controller
    {
        // GET: api/Fine
        [HttpGet]
        public JsonResult Get()
        {
            using (var ctx = new libraryContext())
            {
                return Json(new FineRepository(ctx).GetFines());
                //, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
        }

        // PUT: api/Fine/5
        [HttpPut("{id}")]
        public JsonResult Put(int id)
        {
            using (var ctx = new libraryContext())
            {
                if (new FineRepository(ctx).PayFine(id))
                {
                    ctx.SaveChanges();
                    return Json(new { message = "success" });
                }
                else
                    return Json(new { error = "failed to update the fine, Try again later." });
                //new FineRepository(ctx).PayFine(), new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
        }

        [Route("update")]
        [HttpGet()]
        public JsonResult update() {
            using (var ctx = new libraryContext()) {
                new FineRepository(ctx).updateFines();
                ctx.SaveChanges();
                return this.Get();
            }
        }

        [Route("updateAll/{id}")]
        [HttpGet("{id}")]
        public JsonResult update(string id)
        {
            using (var ctx = new libraryContext())
            {
                new FineRepository(ctx).PayAllFines(id);
                ctx.SaveChanges();
                return this.Get();
            }
        }
    }
}
