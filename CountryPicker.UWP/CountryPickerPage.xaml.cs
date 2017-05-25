using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CountryPicker.UWP.Class.Models;

//Hussein.Juybari@gmail.com
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CountryPicker.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CountryPickerPage : Page
    {
        public  delegate void SelectedCountryEventHandler(object sender, CountryModel selected);

        /// <summary>
        /// Event fire when user click country
        /// </summary>
        public static event SelectedCountryEventHandler SelectedCountry;

        #region Properties

        private string _countryName;

        /// <summary>
        /// Select country with country name
        /// </summary>
        public string CountryName
        {
            get => _countryName;
            set
            {
                _countryName = value;
            }
        }

        #endregion

        public CountryPickerPage()
        {
            this.InitializeComponent();

            Loading += OnLoading;
            Loaded += OnLoaded;
        }

        public CountryPickerPage(string countryName)
        {
            this.InitializeComponent();

            CountryName = countryName;
            
            Loading += OnLoading;
            Loaded += OnLoaded;
        }

        #region Private event methods

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                CountryListView.SelectedIndex = CountryModel.GetCountryModelIndex(_countryName);
                CountryListView.ScrollIntoView(CountryListView.SelectedItem);
            });
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            LoadData(CountryName);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string)
            {
                CountryName = e.Parameter.ToString();
            }
        }

        private void CountryListViewOnItemClick(object sender, ItemClickEventArgs itemClickEventArgs)
        {
            //if (CountryListView.SelectedIndex != -1)
            {
                var model = itemClickEventArgs.ClickedItem as CountryModel;
                if (SelectedCountry != null)
                    SelectedCountry.Invoke(CountryListView, model);
            }
            
        }

        private void SearchBox_OnQueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            CountryVM.Source = CountryModel.GetCountries(args.QueryText);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Load data and show them
        /// </summary>
        /// <param name="countryName"></param>
        private void LoadData(string countryName = "")
        {
            CountryVM.Source = CountryModel.GetCountries();

            CountryListView.ItemClick += CountryListViewOnItemClick;
            CountryListView.IsItemClickEnabled = true;

            _countryName = countryName;
        }

        #endregion
    }
}
