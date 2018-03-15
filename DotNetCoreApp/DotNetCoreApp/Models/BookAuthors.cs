using System;
using System.Collections.Generic;

namespace DotNetCoreApp.Models
{
    public partial class BookAuthors
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Isbn { get; set; }

        public Authors Author { get; set; }
        public Book IsbnNavigation { get; set; }
    }
}
