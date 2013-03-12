using LeadPro.Model;
using LeadPro.Model.ViewModels;
using LeadPro.Phone8.Services;

namespace LeadPro.Phone8
{
    public class ViewModelLocator
    {
        private readonly ICustomerRepository _customerRepository;

        public ViewModelLocator()
        {
            _customerRepository = new PhoneCustomerRepository();
        }

        public HomeViewModel HomeViewModel
        {
            get
            {
                return new HomeViewModel(_customerRepository);
            }
        }

        public EditCustomerViewModel EditCustomerViewModel
        {
            get { return new EditCustomerViewModel(_customerRepository); }
        }
    }
}