using DotNetCoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreApp.Repository
{
    public class FineRepository
    {
        private libraryContext context;
        public FineRepository() { this.context = new libraryContext(); }

        public FineRepository(libraryContext ctx) { this.context = ctx; }

        public List<IGrouping<Borrower, FineData>> GetFines()
        {
            return (from f in context.Fines
                    join bl in context.BookLoans on f.LoanId equals bl.LoanId
                    join bor in context.Borrower on bl.CardId equals bor.CardId
                    join b in context.Book on bl.Isbn equals b.Isbn
                    where !f.Paid
                    select new FineData() { Fine = f, Borrower = bor, Book = b }).ToList().GroupBy(x => x.Borrower).ToList();
        }

        public List<IGrouping<Borrower, FineData>> GetById(int id)
        {
            return (from f in context.Fines
                    join bl in context.BookLoans on f.LoanId equals bl.LoanId
                    join bor in context.Borrower on bl.CardId equals bor.CardId
                    select new FineData() { Fine = f, Borrower = bor }).Where(x => x.Fine.LoanId == id).GroupBy(x => x.Borrower).ToList();
        }

        public void updateFines()
        {
            var query = context.BookLoans.AsQueryable();
            foreach (var bookFine in query)
            {
                var overDue = ((bookFine.DateIn ?? DateTime.Today) - bookFine.DueDate).Days;
                if (overDue > 0)
                {
                    var fine_amt = (decimal)(overDue * 0.25);

                    var fine = context.Fines.FirstOrDefault(x => x.LoanId == bookFine.LoanId);
                    if (fine != null && !fine.Paid)
                        fine.FineAmt = fine_amt;
                    else if (fine == null)
                        context.Fines.Add(new Fines() { LoanId = bookFine.LoanId, FineAmt = fine_amt, Paid = false });
                }
            }
        }

        public bool PayFine(int id)
        {
            return (from f in context.Fines
                    join bl in context.BookLoans on f.LoanId equals bl.LoanId
                    join b in context.Book on bl.Isbn equals b.Isbn
                    where !b.IsCheckedOut && !f.Paid && bl.LoanId == id
                    select f).FirstOrDefault().Paid = true;
        }

        public void PayAllFines(string cardId)
        {
            var query = (from f in context.Fines
                         join bl in context.BookLoans on f.LoanId equals bl.LoanId
                         join b in context.Book on bl.Isbn equals b.Isbn
                         where !b.IsCheckedOut && bl.CardId.Equals(cardId) && !f.Paid
                         select f).AsQueryable();
            foreach (var fine in query)
            {
                fine.Paid = true;
            }
        }

        public class FineData
        {
            public Fines Fine { get; set; }
            public Borrower Borrower { get; set; }
            public Book Book { get; set; }
        }
    }
}
