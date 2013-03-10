using LeadPro.Model;
using Microsoft.Phone.Tasks;

namespace LeadPro.Phone8.Services
{
    class PhoneMappingService : IMappingService
    {
        public void OpenMapForCustomer(Customer customer)
        {
            var mapsTask = new MapsTask();
            mapsTask.SearchTerm = customer.City + ", " + customer.State;
            mapsTask.Show();
        }
    }
}