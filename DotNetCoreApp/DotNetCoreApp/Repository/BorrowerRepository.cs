using DotNetCoreApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreApp.Repository
{
    public class BorrowerRepository
    {
        private libraryContext context;
        public BorrowerRepository() { this.context = new libraryContext(); }

        public BorrowerRepository(libraryContext ctx) { this.context = ctx; }

        public IEnumerable<Borrower> GetBorrower() { return context.Borrower.AsEnumerable(); }

        public IEnumerable<Borrower> GetById(string id) { return GetBorrower().Where(x => x.CardId.Equals(id)); }

        public IEnumerable<Borrower> GetBySsn(string ssn) { return GetBorrower().Where(x => x.Ssn.Equals(ssn)); }

        public string Add(Borrower Borrower)
        {
            if (context.Borrower.Count(x => x.Ssn.Equals(Borrower.Ssn)) > 0)
                return "A user is already registered with SSN: "+Borrower.Ssn+".";
            context.Borrower.Add(Borrower);

            return null;
        }

        public string Add(string ssn, string email, string first_name, string last_name, string address, string city, string state, string phone)
        {
            Borrower borrower = new Borrower();
            borrower.Ssn = ssn;
            borrower.Bname = first_name + "," + last_name;
            borrower.Email = email;
            borrower.Address = address + "," + city + "," + state;
            borrower.Phone = phone;
            return Add(borrower);
        }

        public void Update(string id, string email, string first_name, string last_name, string address, string city, string state, string phone)
        {
            var borrower = GetById(id).FirstOrDefault();
            borrower.Bname = first_name + "," + last_name;
            borrower.Email = email;
            borrower.Address = address + "," + city + "," + state;
            borrower.Phone = phone;
        }

        public void Delete(Borrower bor) { context.Borrower.Remove(bor); }

        public void DeleteById(string id) { Delete(GetById(id).FirstOrDefault()); }

        public void DeleteBySsn(string ssn) { Delete(GetBySsn(ssn).FirstOrDefault()); }
    }
}
