using System;
using System.Collections.Generic;
using LeadPro.ViewModels;
using LeadPro.Win8.Common;
using Windows.UI.Xaml.Controls;

namespace LeadPro.Win8
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HomePage : LayoutAwarePage
    {

        public HomePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var viewModel = new HomeViewModel();
            viewModel.Init();
            DataContext = viewModel;
        }
    }
}
