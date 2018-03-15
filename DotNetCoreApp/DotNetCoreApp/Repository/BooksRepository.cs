using DotNetCoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetCoreApp.Repository
{
    public class BooksRepository
    {
        private libraryContext context;
        private PublisherRepository pubRep;
        private AuthorRepository authRep;
        private BookAuthorRepository bkAuthRep;
        public BooksRepository() { this.context = new libraryContext(); this.pubRep = new PublisherRepository(context); this.authRep = new AuthorRepository(context); this.bkAuthRep = new BookAuthorRepository(context); }

        public BooksRepository(libraryContext ctx) { this.context = ctx; this.pubRep = new PublisherRepository(context); this.authRep = new AuthorRepository(context); this.bkAuthRep = new BookAuthorRepository(context); }

        public IEnumerable<Book> GetBooks() { return context.Book.AsEnumerable(); }

        public IEnumerable<Book> GetByIsbn(string isbn) { return GetBooks().Where(x => x.Isbn.Equals(isbn)); }

        public IEnumerable<BookData> GetBookData()
        {
            return (from b in context.Book
                    join ba in context.BookAuthors on b.Isbn equals ba.Isbn
                    join aut in context.Authors on ba.AuthorId equals aut.AuthorId
                    orderby b.Title
                    select new BookData() { book = b, author = aut });
        }

        public Dictionary<Book, IGrouping<Book, BookData>> GetBookDataByIsbn(string isbn)
        {
            return GetBookData()
                .Where(x => x.book.Isbn.Equals(isbn))
                .ToList().GroupBy(x => x.book)
                .ToDictionary(x => x.Key, x => x);
        }

        public void Add(Book book) { context.Book.Add(book); }

        public string Add(string isbn, string title, string cover, int pages, string publisher, string[] authors)
        {
            if (GetByIsbn(isbn).Count() != 0)
                return "Book with this isbn exists. Cannot add.";

            Book book = new Book();
            book.Isbn = isbn;
            book.Title = title;
            book.Cover = cover;
            book.Pages = pages;

            var pub = pubRep.GetByName(publisher).FirstOrDefault();
            if (pub == null)
                return "Publisher :" + publisher + " not found.";

            book.PublisherId = pub.Id;

            List<Authors> authorList = new List<Authors>();
            foreach (var author in authors)
            {
                var auth = authRep.GetByName(author).FirstOrDefault();
                if (auth == null)
                {
                    return "Author: " + author + " Not found.";
                }
                authorList.Add(auth);
            }
            Add(book);

            bkAuthRep.Add(authorList, isbn);

            return string.Empty;
        }

        public string Update(string isbn, string title, string cover, int pages, string publisher, string[] authors)
        {
            var book = GetByIsbn(isbn).FirstOrDefault();
            book.Title = title;
            book.Pages = pages;
            book.Cover = cover;

            var newPub = pubRep.GetByName(publisher).FirstOrDefault();
            if (newPub == null)
                return "Publisher :" + publisher + " not found.";

            book.PublisherId = newPub.Id;

            bkAuthRep.DeleteByIsbn(isbn);

            List<Authors> authorList = new List<Authors>();
            foreach (var author in authors)
            {
                var auth = authRep.GetByName(author).FirstOrDefault();
                if (auth == null)
                {
                    return "Author: " + author + " Not found.";
                }
                authorList.Add(auth);
            }
            bkAuthRep.Add(authorList, isbn);

            return string.Empty;
        }

        public void markCheckedOut(string isbn)
        {
            GetByIsbn(isbn).FirstOrDefault().IsCheckedOut = true;
        }

        public void markCheckedIn(string isbn)
        {
            GetByIsbn(isbn).FirstOrDefault().IsCheckedOut = false;
        }

        public void Delete(Book book) { bkAuthRep.DeleteByIsbn(book.Isbn); context.Book.Remove(book); }

        public void DeleteByIsbn(string isbn) { Delete(GetByIsbn(isbn).FirstOrDefault()); }

        public List<IGrouping<Book, BookData>> Search(string str)
        {
            var data = (from b in context.Book
                        join ba in context.BookAuthors on b.Isbn equals ba.Isbn
                        join aut in context.Authors on ba.AuthorId equals aut.AuthorId
                        where b.Title.Contains(str) || aut.Name.Contains(str) || b.Isbn.Contains(str)
                        orderby b.Title
                        select new BookData() { book = b, author = aut }).ToList().GroupBy(x => x.book).ToList();
            return data;
        }

        public class BookData
        {
            public Book book { get; set; }
            public Authors author { get; set; }
        }
    }
}