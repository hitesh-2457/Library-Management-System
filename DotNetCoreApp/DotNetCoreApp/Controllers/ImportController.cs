using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreApp.Models;
using DotNetCoreApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Import")]
    public class ImportController : Controller
    {
        // POST: api/Import
        [HttpGet]
        public string get()
        {
            using (var ctx = new libraryContext())
            {
                var AuthorRepo = new AuthorRepository(ctx);
                var PublisherRepo = new PublisherRepository(ctx);
                var BooksRepo = new BooksRepository(ctx);
                try
                {
                    var authorFile = @"E:\OneDrive - The University of Texas at Dallas\Spring18\DB\Projects\project1\books.csv";
                    //var borrowerFile = "";
                    List<Model> list = new List<Model>();
                    var fileStream = new FileStream(authorFile, FileMode.Open, FileAccess.Read);
                    HashSet<string> authors = new HashSet<string>();
                    HashSet<string> publishers = new HashSet<string>();
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        string line;
                        line = streamReader.ReadLine();
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            var temp = line.Split('\t');
                            foreach (var author in temp[3].Split(','))
                            {
                                authors.Add(author);
                            }

                            publishers.Add(temp[5]);

                            int pages = Int16.Parse(temp[6]);
                            var model = new Model(temp[0], temp[2], temp[3].Split(','), temp[4], temp[5], pages);
                            list.Add(model);
                            //BooksRepo.Add(temp[0], temp[2], temp[5], Int16.Parse(temp[6]), temp[4], temp[3].Split(','));
                            //ctx.SaveChanges();
                        }
                    }
                    foreach (var author in authors)
                    {
                        AuthorRepo.Add(author);
                    }
                    foreach (var publisher in publishers)
                    {
                        PublisherRepo.Add(publisher);
                    }
                    ctx.SaveChanges();
                    var count = 0;
                    foreach (var item in list)
                    {
                        count++;
                        BooksRepo.Add(item.isbn, item.title, item.cover, item.pages, item.publisher, item.authors);
                        if (count % 5000 == 0)
                            Console.Write(ctx.SaveChanges());
                    }
                    ctx.SaveChanges();
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                return "Success";
            }
        }
        public class Model
        {
            public string isbn;
            public string title;
            public string[] authors;
            public string cover;
            public string publisher;
            public int pages;
            public Model(string isbn, string title, string[] authors, string cover, string publisher, int pages)
            {
                this.isbn = isbn;
                this.title = title;
                this.authors = authors;
                this.cover = cover;
                this.publisher = publisher;
                this.pages = pages;
            }
        }
    }
}
