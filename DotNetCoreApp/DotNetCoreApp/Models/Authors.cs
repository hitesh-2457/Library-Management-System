using System;
using System.Collections.Generic;

namespace DotNetCoreApp.Models
{
    public partial class Authors
    {
        public Authors()
        {
            BookAuthors = new HashSet<BookAuthors>();
        }

        public int AuthorId { get; set; }
        public string Name { get; set; }

        public ICollection<BookAuthors> BookAuthors { get; set; }
    }
}
