using DotNetCoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreApp.Repository
{
    public class BookLoanRepository
    {
        private libraryContext context;
        public BookLoanRepository() { this.context = new libraryContext(); }

        public BookLoanRepository(libraryContext ctx) { this.context = ctx; }

        public IEnumerable<BookLoanData> GetBookLoans()
        {
            return (from bl in context.BookLoans
                    join bor in context.Borrower on bl.CardId equals bor.CardId
                    join b in context.Book on bl.Isbn equals b.Isbn
                    join f in context.Fines on bl.LoanId equals f.LoanId into a
                    from fi in a.DefaultIfEmpty()
                    where b.IsCheckedOut == true && bl.DateIn == null
                    select new BookLoanData() { bookLoan = bl, borrower = bor, book = b, fine = fi }).AsEnumerable();
        }

        public IEnumerable<BookLoanData> GetByCardId(string id) { return GetBookLoans().Where(x => x.bookLoan.CardId.Equals(id)); }

        public IEnumerable<BookLoanData> GetById(int id) { return GetBookLoans().Where(x => x.bookLoan.LoanId == id); }

        public IEnumerable<BookLoanData> GetByIsbn(string isbn) { return GetBookLoans().Where(x => x.bookLoan.Isbn.Equals(isbn)); }

        public string Add(BookLoans BookLoans)
        {
            if (new BookLoanRepository(context).GetByIsbn(BookLoans.Isbn).Count(x => x.bookLoan.DateIn == null) > 0)
            {
                new BooksRepository(context).markCheckedOut(BookLoans.Isbn);
                context.SaveChanges();
                return "This book is checked out and no longer available.";
            }

            if (new BorrowerRepository(context).GetById(BookLoans.CardId).Count() == 0)
                return "Invalid Card number.";

            var existing = (from bl in context.BookLoans
                            join b in context.Book on bl.Isbn equals b.Isbn
                            where b.IsCheckedOut == true && bl.CardId == BookLoans.CardId
                            select b).ToList();
            if (existing.Count >= 3)
            {
                StringBuilder sb = new StringBuilder();
                existing.ForEach(x => sb.Append(x.Isbn + " "));
                return "Card Holder has 3 books checked out already: " + sb.ToString()
                    + "Can not Check out any more.";
            }

            BookLoans.DateOut = DateTime.Today;
            BookLoans.DueDate = DateTime.Today.AddDays(14);
            new BooksRepository(context).markCheckedOut(BookLoans.Isbn);
            context.BookLoans.Add(BookLoans);
            return null;
        }

        public string Add(string isbn, string cardId)
        {
            BookLoans BookLoan = new BookLoans();
            BookLoan.Isbn = isbn;
            BookLoan.CardId = cardId;
            return Add(BookLoan);
        }

        public void Update(BookLoans bookLoans)
        {
            bookLoans.DateIn = DateTime.Today;
            new BooksRepository(context).markCheckedIn(bookLoans.Isbn);
        }
        public void Update(string isbn, string cardId)
        {
            var bookLoan = GetByCardId(cardId).FirstOrDefault().bookLoan;
            Update(bookLoan);
        }

        public void Update(int id)
        {
            var bookLoan = GetById(id).FirstOrDefault().bookLoan;
            Update(bookLoan);
        }

        public void Delete(BookLoans bor) { context.BookLoans.Remove(bor); }

        public void DeleteById(int id) { Delete(GetById(id).FirstOrDefault().bookLoan); }

        public void DeleteByCardId(string cardId) { Delete(GetByCardId(cardId).FirstOrDefault().bookLoan); }

        public List<BookLoanData> Search(string str)
        {
            return (from bl in context.BookLoans
                    join bor in context.Borrower on bl.CardId equals bor.CardId
                    join b in context.Book on bl.Isbn equals b.Isbn
                    join f in context.Fines on bl.LoanId equals f.LoanId into a
                    from fi in a.DefaultIfEmpty()
                    where (b.Isbn.Contains(str) || bor.CardId.Contains(str) || bor.Bname.Contains(str)) && b.IsCheckedOut == true && bl.DateIn == null
                    select new BookLoanData() { bookLoan = bl, borrower = bor, book = b, fine=fi }).ToList();
        }

        public class BookLoanData
        {
            public BookLoans bookLoan { get; set; }
            public Borrower borrower { get; set; }
            public Book book { get; set; }
            public Fines fine { get; set; }
        }
    }
}
