using System;
using LeadPro.Model;

namespace LeadPro.Win8.Services
{
    class WinRtMappingService : IMappingService
    {
        public async void OpenMapForCustomer(Customer customer)
        {
            var fullAddress = customer.Address1 + " " + customer.Address2 + " " 
                + customer.City + ", " + customer.State +
                              " " + customer.ZipCode;

            var uri = new Uri("bingmaps:?where=" + Uri.EscapeDataString(fullAddress));

            await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}