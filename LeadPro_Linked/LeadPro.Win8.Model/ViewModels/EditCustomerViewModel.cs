using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using LeadPro.Model.Common;
using Microsoft.Phone.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

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
            var cameraCaptureTask = new CameraCaptureTask();
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
            var dialog = new MessageDialog("Would you like to use your camera or select a picture from your library?");
            dialog.Commands.Add(new UICommand("I'd like to use my camera", null, "camera"));
            dialog.Commands.Add(new UICommand("I already have the picture", null, "picker"));

            IStorageFile photoFile;
            var command = await dialog.ShowAsync();
            if ((string) command.Id == "camera")
            {
                var cameraCapture = new CameraCaptureUI();
                photoFile = await cameraCapture.CaptureFileAsync(CameraCaptureUIMode.Photo);
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
            var mapsTask = new MapsTask();
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