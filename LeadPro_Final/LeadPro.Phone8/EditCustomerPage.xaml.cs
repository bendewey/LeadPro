using System;
using System.Windows;
using LeadPro.Model;
using LeadPro.Model.ViewModels;
using Microsoft.Phone.Controls;

namespace LeadPro.Phone8
{
    public partial class EditCustomerPage : PhoneApplicationPage
    {
		public EditCustomerViewModel ViewModel 
        {
            get { return DataContext as EditCustomerViewModel; }
        }
		
        public EditCustomerPage()
        {
            InitializeComponent();
            Loaded += EditCustomerPage_Loaded;
        }

        void EditCustomerPage_Loaded(object sender, RoutedEventArgs e)
        {
            string customerIdValue;
            int customerId;
            if (NavigationContext.QueryString.TryGetValue("customerId", out customerIdValue) 
                && int.TryParse(customerIdValue, out customerId))
            {
                ViewModel.Init(customerId);
            }
            else
            {
                ViewModel.Init(new Customer());
            }
        }

        private void CameraAppBar_Click(object sender, EventArgs e)
        {
        	ViewModel.CaptureImageCommand.Execute(null);
        }

        private void OpenMap_Click(object sender, EventArgs e)
        {
            ViewModel.OpenMapsCommand.Execute(null);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            ViewModel.SaveCustomerCommand.Execute(null);
        }
    }
}