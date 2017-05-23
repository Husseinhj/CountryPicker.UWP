using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CountryPicker.UWP.Class.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CountryPicker.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CountryPickerPage : Page
    {
        public  delegate void SelectedCountryEventHandler(object sender, CountryModel selected);

        public static event SelectedCountryEventHandler SelectedCountry;

        public CountryPickerPage()
        {
            this.InitializeComponent();
        }

        public CountryPickerPage(string countryName)
        {
            this.InitializeComponent();

            CountryName = countryName;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {
                if (e.Parameter is string)
                {
                    CountryName = e.Parameter.ToString();
                }
            }
        }

        private string _countryName;

        public string CountryName
        {
            get => _countryName;
            set
            {
                _countryName = value;
                LoadData(_countryName);
            }
        }

        private void LoadData(string countryName = "")
        {
            CountryVM.Source = CountryModel.GetCountries();

            Loaded += OnLoaded;
            CountryListView.ItemClick += CountryListViewOnItemClick;
            CountryListView.IsItemClickEnabled = true;

            _countryName = countryName;
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

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            CountryListView.SelectedIndex = CountryModel.GetCountryModelIndex(_countryName);
            CountryListView.ScrollIntoView(CountryListView.SelectedItem);
        }

        private void SearchBox_OnQueryChanged(SearchBox sender, SearchBoxQueryChangedEventArgs args)
        {
            CountryVM.Source = CountryModel.GetCountries(args.QueryText);
        }
    }
}
