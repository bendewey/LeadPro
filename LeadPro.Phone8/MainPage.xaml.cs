using System;
using System.Windows;
using System.Windows.Input;
using LeadPro.Model;
using LeadPro.Model.ViewModels;
using Microsoft.Phone.Controls;

namespace LeadPro.Phone8
{
    public partial class MainPage : PhoneApplicationPage
    {
        public HomeViewModel ViewModel
        {
            get { return DataContext as HomeViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.InitAsync();
        }

        private void Customer_Tap(object sender, GestureEventArgs e)
        {
            var customer = ((FrameworkElement) sender).DataContext as Customer;
            if (customer == null)
                return;

            NavigationService.Navigate(new Uri("/EditCustomerPage.xaml?customerId=" + customer.Id, UriKind.Relative));
        }

        private void AddCustomer_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/EditCustomerPage.xaml", UriKind.Relative));
        }
    }
}