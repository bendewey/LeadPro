using System.Collections.Generic;
using System.Linq;
using LeadPro.Model;

namespace LeadPro.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            Customers = new List<Customer>();
        }

        public List<Customer> Customers { get; set; }

        public List<CustomerGroup> CustomerStates
        {
            get
            {
                return (from c in Customers
                        group c by c.State
                            into s
                            select new CustomerGroup()
                            {
                                Title = s.Key,
                                Customers = s.ToList()
                            }).ToList();
            }
        }

        public void Init()
        {
            Customers = GetSampleData();
        }

        public List<Customer> GetSampleData()
        {
            var customers = new List<Customer>();
            customers.Add(new Customer()
            {
                FirstName = "Bob",
                LastName = "Smith",
                State = "New Jersey"
            });
            customers.Add(new Customer()
            {
                FirstName = "John",
                LastName = "Doe",
                State = "New Jersey"
            });
            return customers;
        }
    }
}