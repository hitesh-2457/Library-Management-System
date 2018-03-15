using System;
using System.Collections.Generic;

namespace DotNetCoreApp.Models
{
    public partial class Fines
    {
        public int LoanId { get; set; }
        public decimal FineAmt { get; set; }
        public bool Paid { get; set; }

        public BookLoans Loan { get; set; }
    }
}
