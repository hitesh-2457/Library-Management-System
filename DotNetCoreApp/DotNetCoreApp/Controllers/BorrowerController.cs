using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreApp.Models;
using DotNetCoreApp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Borrower")]
    public class BorrowerController : Controller
    {
        [HttpPost]
        public JsonResult Post([FromBody]Borrower borrower)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                try
                {
                    conn.ConnectionString = @"Server=Hitesh-PC\SQLEXPRESS;Database=library;Trusted_Connection=true";
                    var query = @"Insert into borrower (ssn, bname, address, phone, email) values (@ssn,@bname,@address,@phone,@email);";

                    SqlCommand command = new SqlCommand(query, conn);
                    command.Parameters.Add(new SqlParameter("@ssn", borrower.Ssn));
                    command.Parameters.Add(new SqlParameter("@bname", borrower.Bname));
                    command.Parameters.Add(new SqlParameter("@address", borrower.Address));
                    command.Parameters.Add(new SqlParameter("@phone", borrower.Phone));
                    command.Parameters.Add(new SqlParameter("@email", borrower.Email));

                    conn.Open();

                    command.ExecuteNonQuery();

                    return Json(new { message = "success" });
                }
                catch (Exception e)
                {
                    return Json(new { error = "Failed to execute the query." , exception = e.Message});
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}