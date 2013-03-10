using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadPro.Model
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> Get();
        Task<Customer> GetById(int customerId);
        void Save(Customer customer);
    }
}