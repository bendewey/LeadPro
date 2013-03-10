using LeadPro.Model;
using LeadPro.Model.ViewModels;
using LeadPro.Win8.Services;

namespace LeadPro.Win8
{
    public class ViewModelLocator
    {
        private readonly ICustomerRepository _customerRepository;

        public ViewModelLocator()
        {
            _customerRepository = new HttpClientCustomerRepository();
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
            get
            {
                return new EditCustomerViewModel(_customerRepository, new WinRtImageCaptureService(), new WinRtMappingService());
            }
        }
    }
}
