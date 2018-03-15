using DotNetCoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreApp.Repository
{
    public class BookAuthorRepository
    {
        private libraryContext context;
        public BookAuthorRepository() { this.context = new libraryContext(); }

        public BookAuthorRepository(libraryContext ctx) { this.context = ctx; }

        public IEnumerable<Authors> GetAuthors(string isbn)
        {
            return from au in context.Authors
                   join bkAuth in context.BookAuthors
                   on au.AuthorId equals bkAuth.AuthorId
                   where bkAuth.Isbn.Equals(isbn)
                   select au;
        }

        public IEnumerable<Book> GetBooks(int authorId)
        {
            return from bk in context.Book
                   join bkAuth in context.BookAuthors
                   on bk.Isbn equals bkAuth.Isbn
                   where bkAuth.AuthorId == authorId
                   select bk;
        }

        public IEnumerable<BookAuthors> GetByIsbn(string isbn) { return context.BookAuthors.Where(x => x.Isbn.Equals(isbn)).AsEnumerable(); }

        public void Add(List<Authors> Author, string isbn) { Author.ForEach(a => context.BookAuthors.Add(new BookAuthors() { AuthorId = a.AuthorId, Isbn = isbn })); }

        public void Delete(BookAuthors bkAuth) { context.BookAuthors.Remove(bkAuth); }

        public void Delete(List<BookAuthors> bkAuths) { bkAuths.ForEach(x => Delete(x)); }

        public void DeleteByIsbn(string isbn) { Delete(GetByIsbn(isbn).ToList()); }
    }
}
