using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadPro.Model.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly ICustomerRepository _customerRepository;
        private List<Customer> _customers;

        public HomeViewModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
            Customers = new List<Customer>();
        }

        public List<Customer> Customers
        {
            get { return _customers; }
            set
            {
                SetProperty(ref _customers, value, "Customers");
                OnPropertyChanged("CustomerStates");
            }
        }

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

        public void InitAsync()
        {
            _customerRepository.Get()
                .ContinueWith(t => Customers = t.Result, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}