using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using LeadPro.Model.Common;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace LeadPro.Model.ViewModels
{
    public class EditCustomerViewModel : ViewModelBase
    {
        private readonly ICustomerRepository _customerRepository;
        private CustomerViewModel _customer;

        public EditCustomerViewModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

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

#if WINDOWS_PHONE
        public void CaptureImage()
        {
            var cameraCaptureTask = new Microsoft.Phone.Tasks.CameraCaptureTask();
            cameraCaptureTask.Completed += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        Customer.ImageStream = e.ChosenPhoto;
                    }
                };
            
            cameraCaptureTask.Show();
        }
#else
        public async void CaptureImage()
        {
            var dialog = new Windows.UI.Popups.MessageDialog("Would you like to use your camera or select a picture from your library?");
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("I'd like to use my camera", null, "camera"));
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("I already have the picture", null, "picker"));

            IStorageFile photoFile;
            var command = await dialog.ShowAsync();
            if ((string) command.Id == "camera")
            {
                var cameraCapture = new Windows.Media.Capture.CameraCaptureUI();
                photoFile = await cameraCapture.CaptureFileAsync(Windows.Media.Capture.CameraCaptureUIMode.Photo);
            }
            else
            {
                var photoPicker = new FileOpenPicker();
                photoPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                photoPicker.FileTypeFilter.Add(".png");
                photoPicker.FileTypeFilter.Add(".jpg");
                photoPicker.FileTypeFilter.Add(".jpeg");

                photoFile = await photoPicker.PickSingleFileAsync();
            }

            if (photoFile == null)
                return;

            var raStream = await photoFile.OpenAsync(FileAccessMode.Read);
            Customer.ImageStream = raStream.AsStream();
        }
#endif

#if WINDOWS_PHONE
        public void OpenMaps()
        {
            var mapsTask = new Microsoft.Phone.Tasks.MapsTask();
            mapsTask.SearchTerm = Customer.City + ", " + Customer.State;
            mapsTask.Show();
        }
#else
        public async void OpenMaps()
        {
            var fullAddress = Customer.Address1 + " " + Customer.Address2 + " "
                + Customer.City + ", " + Customer.State +
                              " " + Customer.ZipCode;

            var uri = new Uri("bingmaps:?where=" + Uri.EscapeDataString(fullAddress));

            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
#endif

        public void SaveCustomer()
        {
            _customerRepository.Save(Customer.AsCustomer());
        }
    }
}