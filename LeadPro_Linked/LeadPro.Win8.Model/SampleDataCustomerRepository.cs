using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadPro.Model
{
    public class SampleDataCustomerRepository : ICustomerRepository
    {
        private readonly TaskCompletionSource<List<Customer>> _customersCompletionTask;

        private readonly TaskCompletionSource<Customer> _customerCompletionTask;

        public SampleDataCustomerRepository()
        {
            var customers = GetSampleData();
            _customersCompletionTask = new TaskCompletionSource<List<Customer>>();
            _customersCompletionTask.SetResult(customers);

            _customerCompletionTask = new TaskCompletionSource<Customer>();
            _customerCompletionTask.SetResult(customers[0]);
        }

        public Task<List<Customer>> Get()
        {
            return _customersCompletionTask.Task;
        }

        public Task<Customer> GetById(int customerId)
        {
            return _customerCompletionTask.Task;
        }

        public void Save(Customer customer)
        {
            throw new System.NotImplementedException();
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