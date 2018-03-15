using System;
using System.Collections.Generic;

namespace DotNetCoreApp.Models
{
    public partial class BookLoans
    {
        public int LoanId { get; set; }
        public string Isbn { get; set; }
        public string CardId { get; set; }
        public DateTime DateOut { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? DateIn { get; set; }

        public Borrower Card { get; set; }
        public Book IsbnNavigation { get; set; }
        public Fines Fines { get; set; }
    }
}
