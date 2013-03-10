using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using LeadPro.WebApi.Models;

namespace LeadPro.WebApi.Controllers
{
    public class CustomersController : ApiController
    {
        private static List<Customer> _customers;

        static CustomersController()
        {
            _customers = new List<Customer>();
            _customers.Add(new Customer()
            {
                Id=1,
                FirstName = "John",
                LastName = "Doe",
                City = "New York",
                State = "NY",
                ImageBase64 = GetImageFromFile("~/Images/Data/JohnDoe.jpg")
            });
            _customers.Add(new Customer()
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Doe",
                City = "New York",
                State = "NY",
                ImageBase64 = GetImageFromFile("~/Images/Data/JaneDoe.jpg")
            });
        }

        public static string GetImageFromFile(string path)
        {
            var filePath = HttpContext.Current.Request.MapPath(path);
            var bytes = File.ReadAllBytes(filePath);

            return Convert.ToBase64String(bytes);
        }

        // GET api/customers
        public IEnumerable<Customer> Get()
        {
            return _customers;
        }

        // GET api/customers/5
        public Customer Get(long id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        // POST api/customers
        public void Post([FromBody]Customer value)
        {
            if (value == null)
                return;

            value.Id = _customers.Max(c => c.Id) + 1;

            _customers.Add(value);
        }

        // PUT api/customers/5
        public void Put(long id, [FromBody]Customer value)
        {
            if (value == null)
                return;

            var index = _customers.FindIndex(c => c.Id == id);
            if (index != -1)
            {
                _customers[index] = value;    
            }
            else
            {
                _customers.Add(value);
            }
        }

        // DELETE api/customers/5
        public void Delete(long id)
        {
        }
    }
}