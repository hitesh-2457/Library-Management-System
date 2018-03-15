using System;
using System.Collections.Generic;

namespace DotNetCoreApp.Models
{
    public partial class Book
    {
        public Book()
        {
            BookAuthors = new HashSet<BookAuthors>();
            BookLoans = new HashSet<BookLoans>();
        }

        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Cover { get; set; }
        public int Pages { get; set; }
        public int PublisherId { get; set; }
        public bool IsCheckedOut { get; set; }

        public Publisher Publisher { get; set; }
        public ICollection<BookAuthors> BookAuthors { get; set; }
        public ICollection<BookLoans> BookLoans { get; set; }
    }
}
