using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetCoreApp.Models
{
    public partial class Borrower
    {
        public Borrower()
        {
            BookLoans = new HashSet<BookLoans>();
        }

        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CardId { get; set; }
        public string Ssn { get; set; }
        public string Bname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public ICollection<BookLoans> BookLoans { get; set; }
    }
}
