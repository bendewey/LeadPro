using System.Threading.Tasks;
using System.Windows.Input;
using LeadPro.Model.Common;

namespace LeadPro.Model.ViewModels
{
    public class EditCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IImageCaptureService _imageCaptureService;
        private readonly IMappingService _mappingService;
        private CustomerViewModel _customer;

        public EditCustomerViewModel(ICustomerRepository customerRepository, IImageCaptureService imageCaptureService, IMappingService mappingService)
        {
            _customerRepository = customerRepository;
            _imageCaptureService = imageCaptureService;
            _mappingService = mappingService;

            CaptureImageCommand = new DelegateCommand(CaptureImage);
            OpenMapsCommand = new DelegateCommand(OpenMaps);
            SaveCustomerCommand = new DelegateCommand(SaveCustomer);
        }

        public CustomerViewModel Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value, "Customer"); }
        }

        public ICommand CaptureImageCommand { get; set; }
        public ICommand OpenMapsCommand { get; set; }
        public ICommand SaveCustomerCommand { get; set; }

        public void Init(int customerId)
        {
            _customerRepository.GetById(customerId)
                .ContinueWith(t => Init(t.Result), TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Init(Customer customer)
        {
            Customer = new CustomerViewModel(customer);
        }

        public void CaptureImage()
        {
            _imageCaptureService.CaptureAsync()
                .ContinueWith(t => Customer.ImageStream = t.Result, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void OpenMaps()
        {
            _mappingService.OpenMapForCustomer(Customer.AsCustomer());
        }

        public void SaveCustomer()
        {
            _customerRepository.Save(Customer.AsCustomer());
        }
    }
}