using System.Collections.Generic;

namespace LeadPro.Model
{
    public class Customer
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string ImageBase64 { get; set; }
    }

    public class CustomerGroup
    {
        public string Title { get; set; }
        public List<Customer> Customers { get; set; }
    }
}